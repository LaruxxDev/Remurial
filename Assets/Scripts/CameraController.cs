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



    [Header("Configuración")]
    public int aimPriority = 20; // Prioridad alta al apuntar
    public int defaultPriority = 9; // Prioridad baja al dejar de apuntar
    public int coldownFoto = 2; // Tiempo de espera entre cada foto (en segundos)
    public float reavelTime = 5f; // Tiempo que tarda en revelarse la foto
    private string rutaFotos; // Ruta donde se guardarán las fotos

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
        
        if (attackAction.action.WasPressedThisFrame() && aimCamera.Priority == aimPriority && Time.time >= tiempoProximaFoto)
        {
            contadorFotos++;
            tiempoProximaFoto = Time.time + coldownFoto;
            if (toggleFlash)
            {                
                Flashing();
            }
            Flashing();
            StartCoroutine(ProcesoTomarFoto());
        }
        else
        {
            if (flashAction.action.WasPressedThisFrame() /*&& aimCamera.Priority != aimPriority*/ && Time.time >= tiempoProximaFoto || toggleFlash)
            {
                Debug.Log("No se puede tomar foto: No estás apuntando.");
                tiempoProximaFoto = Time.time + coldownFoto;
                // Realiza el flash
                ResetearRotacionCameraMesh();
                Flashing();

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


        Texture2D fotoCapturada = ScreenCapture.CaptureScreenshotAsTexture();

        Debug.Log("¡Flash! Foto tomada.");

        // 1. ESPERA CRÍTICA: Debemos esperar a que Unity termine de dibujar todo este frame
        yield return new WaitForEndOfFrame();

        // 4. INSTANCIACIÓN Y ASIGNACIÓN
        if (prefabFotoFisica != null && puntoDeAparicion != null)
        {
            // Creamos el prefab en la posición y rotación del punto de aparición
            GameObject fotoInstanciada = Instantiate(prefabFotoFisica, puntoDeAparicion.position, puntoDeAparicion.rotation);
            fotoInstanciada.transform.SetParent(puntoDeAparicion); 
            
            // Buscamos el MeshRenderer 
            MeshRenderer meshRenderer = fotoInstanciada.GetComponentInChildren<MeshRenderer>();

            if (meshRenderer != null)
            {
                // Le aplicamos el nuevo material al Quad
                meshRenderer.material.mainTexture = fotoCapturada;
                Debug.Log("¡Foto creada físicamente y material aplicado al Quad!");
                GuardarFoto(fotoInstanciada);
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

    public void GuardarFoto(GameObject foto)
    {
        if (foto != null)
        {
            string idFoto = "Foto_" + contadorFotos;

            string nombreArchivo = idFoto + ".png";
            string rutaCompleta = Path.Combine(rutaFotos, nombreArchivo);

            // Asegurarse de que la carpeta exista
            if (!Directory.Exists(rutaFotos))
            {
                Directory.CreateDirectory(rutaFotos);
            }

            // Guardar la textura de la foto en un archivo PNG
            Texture2D texturaFoto = ((MeshRenderer)foto.GetComponentInChildren<MeshRenderer>()).material.mainTexture as Texture2D;
            byte[] bytes = texturaFoto.EncodeToPNG();
            File.WriteAllBytes(rutaCompleta, bytes);

            // Agregar la foto al inventario
            DatosFotos nuevaFoto = new DatosFotos(idFoto, rutaFotos + "Foto_" + contadorFotos + ".png", reavelTime);
            //GestorInventario.Instance.AgregarFoto(nuevaFoto);

            //Destroy(texturaFoto); // Liberar la textura de la memoria
            //Destroy(foto); // Eliminar la foto del juego después de guardarla

            Debug.Log("Foto guardada en: " + rutaCompleta);
        }
        else
        {
            Debug.LogError("No se puede guardar una foto nula.");
        }
    }
    /*
    private void DetectarEnemigos()
    {
        //Collider[] objetosCapturados = Physics.OverlapBox(centroMundo, mitadTamano, zonaDeFoto.transform.rotation, capaEnemigos);
        if (objetosCapturados.Length > 0)
        {
            foreach (Collider obj in objetosCapturados)
            {
                Debug.Log($"<color=cyan>¡FOTOGRAFIADO: {obj.gameObject.name}!</color>");
                
                // --- AQUÍ PONES LO QUE QUIERAS QUE PASE ---
                // Por ejemplo, si es un enemigo tipo Fatal Frame, lo destruyes o le haces daño:
                if (obj.CompareTag("Enemy"))
                {
                    Debug.Log("¡Enemigo destruido por el flash!");
                    Destroy(obj.gameObject);
                }
            }
        }
        else
        {
            Debug.Log("No fotografiaste nada especial.");
        }
    }*/
    #endregion
}