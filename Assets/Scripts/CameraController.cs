using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Collections;

public class CameraController : MonoBehaviour
{
    #region Values
    [Header("Cámaras")]
    [SerializeField] private CinemachineCamera aimCamera; 
    [SerializeField] private GameObject CameraMesh; 

    private CinemachinePanTilt panTiltComponent;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference attackAction; 
    [SerializeField] private InputActionReference aimAction; 
    [SerializeField] private InputActionReference flashAction; 
    [SerializeField] private InputActionReference toggleFlashAction; 
    [SerializeField] private InputActionReference saveAction; 



    [Header("Configuración")]
    public int aimPriority = 20; // Prioridad alta al apuntar
    public int defaultPriority = 9; // Prioridad baja al dejar de apuntar
    public int coldownFoto = 2; // Tiempo de espera entre cada foto (en segundos)
    public float reavelTime = 5f; // Tiempo que tarda en revelarse la foto
    private string rutaFotos; // Ruta donde se guardarán las fotos

    [Header("Detección de Enemigos")]
    [SerializeField] private Vector3 sizeZonaFoto = new Vector3(3f, 2f, 5f); // Ancho, alto, profundidad
    [SerializeField] private LayerMask capaEnemigos; // Asigna la layer de enemigos en el Inspector

    [Header("Configuración del Prefab")]
    [SerializeField] private GameObject prefabFotoFisica; 
    [SerializeField] private Transform puntoDeAparicion;

    [Header("Interaction")]
    public InspectSystem inspectSystem; 
    [SerializeField] private BoxCollider objetosCapturados;

    [Header("Configuración de Flash")]
    [SerializeField] private float flashDuration = 3.0f; // Duración del flash en segundos
    [SerializeField] private GameObject flashEffect; // Efecto visual para el flash 
    [SerializeField] private Light luzFlash; // Luz para el efecto de flash
    [SerializeField] private float intensidadMaximaFlash = 122f; // Intensidad máxima de la luz durante el flash
    [SerializeField] private float duracionApagadoFlash = 1f; // Tiempo que tarda en apagarse la luz después del flash


    private int contadorFotos = 0; //GestorInventario.Instance.fotosEnInventario.Count; // Contador para nombrar las fotos de forma única
    private float tiempoProximaFoto = 0f;
    private bool toggleFlash = true; // Para alternar el flash
    private GameObject fotoFisica = null; // Foto física instanciada en el mundo

    #endregion
    #region Unity Methods

    // CUANDO EXPORTEMOS LA CARPETA ASSETS NO SALDRÁN LOS ARCHIVOS DE FOTOS, POR ESO GUARDAMOS EN PERSISTENTDATA

    void Awake()
    {
        // Guardaremos las fotos en una subcarpeta llamada "AlbumPolaroid"
        rutaFotos = Path.Combine(Application.persistentDataPath, "AlbumPolaroid");

        if (!Directory.Exists(rutaFotos))
        {
            Directory.CreateDirectory(rutaFotos);
            Debug.Log("¡Carpeta creada por primera vez! Ruta: " + rutaFotos);
        }
        else
        {
            Debug.Log("La carpeta ya existía en: " + rutaFotos);
        }
    }
    

    void Start()
    {
        if (aimCamera != null)
        {
            panTiltComponent = aimCamera.GetComponent<CinemachinePanTilt>();
        }
        if (flashEffect != null)
        {
            flashEffect.SetActive(false); 
        }
    }

    void Update()
    {
        if (saveAction.action.WasPressedThisFrame())
        {
            if (fotoFisica != null)
            {
                // Guardar la foto actual en el inventario
                MeshRenderer meshRenderer = fotoFisica.GetComponentInChildren<MeshRenderer>();
                if (meshRenderer == null)
                {
                    Debug.LogWarning("La foto física no tiene MeshRenderer.");
                    return;
                }

                Texture2D textura = meshRenderer.material.mainTexture as Texture2D;
                if (textura == null)
                {
                    Debug.LogWarning("La foto física no tiene textura asignada.");
                    return;
                }
                GuardarFoto(fotoFisica, textura); // Pasamos null porque la textura ya está guardada en el proceso de tomar foto
            }
            else
            {
                Debug.LogWarning("No hay foto física para guardar.");
            }
        }

        // Alternar el flash con el botón asignado
        if (toggleFlashAction.action.WasPressedThisFrame())
        {
            toggleFlash = !toggleFlash;
        }

        if (aimAction.action.WasPressedThisFrame())
        {
            aimCamera.Priority = aimPriority;
            ResetearRotacionCameraMesh();
        }
        else if (aimAction.action.WasReleasedThisFrame())
        {
            if (panTiltComponent != null)
            {
                panTiltComponent.PanAxis.Value = 0f;
                panTiltComponent.TiltAxis.Value = 0f;
            }            
            aimCamera.Priority = defaultPriority;
            Debug.Log("CameraMesh localRotation set to zero: " + CameraMesh.transform.localRotation);
        }
        bool estaApuntando = aimCamera.Priority == aimPriority;
        bool cooldownListo = Time.time >= tiempoProximaFoto;

        if (attackAction.action.WasPressedThisFrame() && estaApuntando)
        {
            if (cooldownListo)
            {
                contadorFotos++;
                tiempoProximaFoto = Time.time + coldownFoto;
                if (toggleFlash)
                {                
                    Flashing();
                }
                StartCoroutine(ProcesoTomarFoto());
                
            }
            else
            {
                Debug.Log("No se puede tomar foto: En coldown. Tiempo restante: " + (tiempoProximaFoto - Time.time).ToString("F2") + " segundos.");
            }
        }
        else
        {
            if (flashAction.action.WasPressedThisFrame() /*&& aimCamera.Priority != aimPriority*/ && !estaApuntando)
            {
                if (cooldownListo)
                {
                    tiempoProximaFoto = Time.time + coldownFoto;
                    // Realiza el flash
                    ResetearRotacionCameraMesh();
                    Flashing();  
                }
                else
                {
                    Debug.Log("No se hacer flash: En coldown. Tiempo restante: " + (tiempoProximaFoto - Time.time).ToString("F2") + " segundos.");
                }
                

            }
            else if (attackAction.action.WasPressedThisFrame() && Time.time < tiempoProximaFoto)
            {
                Debug.Log("No se puede tomar foto: En coldown. Tiempo restante: " + (tiempoProximaFoto - Time.time).ToString("F2") + " segundos.");
            }
        }
    }
    #endregion
    private void ResetearRotacionCameraMesh()
    {
        if (CameraMesh != null)
        {
            CameraMesh.transform.localRotation = Quaternion.identity;
            if (panTiltComponent != null)
            {
                panTiltComponent.PanAxis.Value = 0f;
                panTiltComponent.TiltAxis.Value = 0f;
            } 
            Debug.Log("CameraMesh localRotation reset to zero: " + CameraMesh.transform.localRotation);
        }
        else
        {
            Debug.LogError("CameraMesh no asignada en el Inspector.");
        }
    }
    #region Flash Effect
    public void Flashing()
    {
        if (flashEffect != null)
        {
            flashEffect.SetActive(true);
        }
        if (luzFlash != null)
        {
            luzFlash.enabled = true;
            luzFlash.intensity = intensidadMaximaFlash;
        }
        StartCoroutine(SecuenciaDesactivarFlash());
    }

    private IEnumerator SecuenciaDesactivarFlash()
    {
        // Mantenemos el flash en su punto máximo durante este tiempo
        yield return new WaitForSeconds(flashDuration);

        // Pasado ese tiempo, apagamos el efecto visual de golpe
        if (flashEffect != null)
        {
            flashEffect.SetActive(false);
        }

        // Iniciamos la corrutina para que la luz 3D baje poco a poco
        StartCoroutine(DesvanecerFlash());
    }

    private IEnumerator DesvanecerFlash()
    {
        if (luzFlash == null) yield break;

        float tiempoPasado = 0f;

        // Mientras el tiempo que ha pasado sea menor a la duración que queremos...
        while (tiempoPasado < duracionApagadoFlash)
        {
            tiempoPasado += Time.deltaTime;
            
            // Lerp mezcla dos valores. Va de intensidadMaximaFlash a 0 a lo largo del tiempo.
            luzFlash.intensity = Mathf.Lerp(intensidadMaximaFlash, 0f, tiempoPasado / duracionApagadoFlash);
            
            // Esperamos al siguiente frame para seguir bajando la intensidad
            yield return null; 
        }

        // Por seguridad, al terminar nos aseguramos de que quede exactamente en 0
        luzFlash.intensity = 0f;
        luzFlash.enabled = false;
    }
    #endregion

    #region Foto e Interacción

    private IEnumerator ProcesoTomarFoto()
    {
        Debug.Log("¡Flash! Iniciando captura...");

        yield return new WaitForEndOfFrame();

        Texture2D fotoCapturada = ScreenCapture.CaptureScreenshotAsTexture();

        DetectarYEliminarEnemigos();

        Debug.Log("¡Flash! Foto tomada.");

        if (fotoFisica != null)
        {
            GuardarFoto(fotoFisica, fotoCapturada);
            fotoFisica = null;
        }        
        // 4. INSTANCIACIÓN Y ASIGNACIÓN
        if (prefabFotoFisica != null && puntoDeAparicion != null)
        {
            // Creamos el prefab en la posición y rotación del punto de aparición
            GameObject fotoInstanciada = Instantiate(prefabFotoFisica, puntoDeAparicion.position, puntoDeAparicion.rotation);
            fotoInstanciada.transform.SetParent(puntoDeAparicion); 

            fotoFisica = fotoInstanciada; // Guardamos la referencia para futuras interacciones
            
            // Buscamos el MeshRenderer 
            MeshRenderer meshRenderer = fotoInstanciada.GetComponentInChildren<MeshRenderer>();

            if (meshRenderer != null)
            {
                // Le aplicamos el nuevo material al Quad
                meshRenderer.material.mainTexture = fotoCapturada;
                Debug.Log("¡Foto creada físicamente y material aplicado al Quad!");
                //GuardarFoto(fotoInstanciada,fotoCapturada);
                //InteractuarConFoto(fotoInstanciada);
            }
            else
            {
                Debug.LogError("El prefab de la foto no tiene un MeshRenderer ");
            }
        }
        else
        {
            Debug.LogError("Falta asignar el Prefab de la foto o el Punto de Aparición en el Inspector.");
        }
    }

    public void InteractuarConFoto(GameObject foto)
    {
        if (foto != null)
        {
            Debug.Log("Interacción con la foto: " + foto.name);
            inspectSystem.EnterInspectionMode(foto);
        }
        else
        {
            Debug.LogError("No se puede interactuar con una foto nula.");
        }
    }

    public void GuardarFoto(GameObject foto, Texture2D texturaFoto)
    {
        if (foto == null)
        {
            Debug.LogError("No se puede guardar una foto nula.");
            return;
        }

        if (texturaFoto == null)
        {
            Debug.LogError("La textura es nula.");
            return;
        }

        string idFoto = "Foto_" + contadorFotos;
        string rutaCompleta = Path.Combine(rutaFotos, idFoto + ".png");

        // Asegurarse de que la carpeta exista
        if (!Directory.Exists(rutaFotos))
        {
            Directory.CreateDirectory(rutaFotos);
        }

        // Guardar el PNG en disco
        byte[] bytes = texturaFoto.EncodeToPNG();
        File.WriteAllBytes(rutaCompleta, bytes);

        // Crear los datos con la ruta correcta y la textura ya cargada en memoria
        PhotoData nuevaFoto = new PhotoData(idFoto, rutaCompleta, reavelTime);
        Item itemFoto = new Item
        {
            name = idFoto,
            description = "Una foto tomada el " + System.DateTime.Now.ToString("dd/MM/yyyy"),
            esFoto = true,
            datosFoto = nuevaFoto,
            prefabItem = prefabFotoFisica
        };
        bool agregada = InventarioManager.Instance.AgregarItem(itemFoto);

       if (agregada)
        {
            // Liberar la textura de memoria, ya está guardada en disco
            Destroy(texturaFoto);
            Debug.Log("Foto guardada y añadida al inventario: " + rutaCompleta);
        }
        else
        {
            Debug.LogWarning("No se pudo agregar al inventario (¿lleno?).");
            Destroy(texturaFoto);
        }
        Destroy(foto); // Destruir la foto física del mundo después de guardarla
        Debug.Log("Foto guardada y añadida al inventario: " + rutaCompleta);

    }
    
    private void DetectarYEliminarEnemigos()
    {
        // Usamos la posición y rotación de la cámara aim como origen del box
        Vector3 centro = aimCamera.transform.position + aimCamera.transform.forward * (sizeZonaFoto.z / 2f);
        Quaternion rotacion = aimCamera.transform.rotation;

        Collider[] objetosDetectados = Physics.OverlapBox(centro, sizeZonaFoto / 2f, rotacion, capaEnemigos);

        if (objetosDetectados.Length > 0)
        {
            foreach (Collider col in objetosDetectados)
            {
                if (col.CompareTag("Enemy"))
                {
                    Debug.Log($"<color=cyan>¡Enemigo fotografiado y destruido: {col.gameObject.name}!</color>");
                    EnemyCollision enemyCollision = col.GetComponent<EnemyCollision>();
                    if (enemyCollision != null)
                    {
                        enemyCollision.PHOTOMADE = true;
                    }

                    //Destroy(col.gameObject);
                }
            }
        }
        else
        {
            Debug.Log("No había enemigos en el encuadre.");
        }
    }
    #endregion

    private void OnDrawGizmosSelected()
    {
        if (aimCamera == null) return;

        Gizmos.color = Color.cyan;
        Vector3 centro = aimCamera.transform.position + aimCamera.transform.forward * (sizeZonaFoto.z / 2f);
        
        // Gizmos no soporta rotación directamente, usamos matriz
        Gizmos.matrix = Matrix4x4.TRS(centro, aimCamera.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, sizeZonaFoto);
        Gizmos.matrix = Matrix4x4.identity;
    }

}