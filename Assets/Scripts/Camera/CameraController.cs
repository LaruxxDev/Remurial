using Unity.Cinemachine;
using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class CameraController : MonoBehaviour
{
    #region Values
    [Header("Cámaras")]
    [SerializeField] private CinemachineCamera aimCamera;
    [SerializeField] private GameObject CameraMesh;

    private CinemachinePanTilt panTiltComponent;

    [Header("Input")]
    [SerializeField] private GameInputReader _input;

    [Header("Configuración")]
    public int aimPriority = 20;
    public int defaultPriority = 9;
    public int coldownFoto = 2;
    public float reavelTime = 5f;
    private string rutaFotos;

    [Header("Detección de Enemigos")]
    [SerializeField] private Vector3 sizeZonaFoto = new Vector3(3f, 2f, 5f);
    [SerializeField] private LayerMask capaEnemigos;
    [SerializeField] private DetectorEnemigosEnFoto detector;

    [Header("Configuración del Prefab")]
    [SerializeField] private GameObject prefabFotoFisica;
    [SerializeField] private Transform puntoDeAparicion;

    [Header("Interaction")]
    public InspectSystem inspectSystem;
    [SerializeField] private BoxCollider objetosCapturados;

    [Header("Configuración de Flash")]
    [SerializeField] private float flashDuration = 3.0f;
    [SerializeField] private GameObject flashEffect;
    [SerializeField] private Light luzFlash;
    [SerializeField] private float intensidadMaximaFlash = 122f;
    [SerializeField] private float duracionApagadoFlash = 1f;

    private int contadorFotos = 100;
    private float tiempoProximaFoto = 0f;
    private bool toggleFlash = true;
    private GameObject fotoFisica = null;
    private Texture2D texturaFoto = null;
    // Estado interno
    private bool _estaApuntando = false;
    #endregion

    #region Unity Methods

    void Awake()
    {
        rutaFotos = Path.Combine(Application.persistentDataPath, "AlbumPolaroid");
        if (!Directory.Exists(rutaFotos))
        {
            Directory.CreateDirectory(rutaFotos);
            Debug.Log("¡Carpeta creada! Ruta: " + rutaFotos);
        }
    }

    void Start()
    {
        if (aimCamera != null)
            panTiltComponent = aimCamera.GetComponent<CinemachinePanTilt>();

        if (flashEffect != null)
            flashEffect.SetActive(false);
    }

    private void OnEnable()
    {
        _input.OnAttackStarted  += HandleAttack;
        _input.OnAimStarted     += HandleAimStart;
        _input.OnAimCanceled    += HandleAimStop;
        _input.OnFlashStarted   += HandleFlash;
        _input.OnToggleFlash    += HandleToggleFlash;
        _input.OnSave           += HandleSave;
    }

    private void OnDisable()
    {
        _input.OnAttackStarted  -= HandleAttack;
        _input.OnAimStarted     -= HandleAimStart;
        _input.OnAimCanceled    -= HandleAimStop;
        _input.OnFlashStarted   -= HandleFlash;
        _input.OnToggleFlash    -= HandleToggleFlash;
        _input.OnSave           -= HandleSave;
    }

    #endregion

    #region Input Handlers

    private void HandleAimStart()
    {
        _estaApuntando = true;
        aimCamera.Priority = aimPriority;
        ResetearRotacionCameraMesh();
    }

    private void HandleAimStop()
    {
        _estaApuntando = false;
        aimCamera.Priority = defaultPriority;
        ResetearRotacionCameraMesh();
    }

    private void HandleAttack()
    {
        if (!_estaApuntando) return;

        bool cooldownListo = Time.time >= tiempoProximaFoto;
        if (cooldownListo)
        {
            contadorFotos++;
            tiempoProximaFoto = Time.time + coldownFoto;
            if (toggleFlash) Flashing();
            StartCoroutine(ProcesoTomarFoto());
        }
        else
        {
            Debug.Log("Cooldown restante: " + (tiempoProximaFoto - Time.time).ToString("F2") + "s");
        }
    }

    private void HandleFlash()
    {
        if (_estaApuntando) return; // El flash manual solo funciona sin apuntar

        bool cooldownListo = Time.time >= tiempoProximaFoto;
        if (cooldownListo)
        {
            tiempoProximaFoto = Time.time + coldownFoto;
            Flashing();
        }
        else
        {
            Debug.Log("Cooldown flash restante: " + (tiempoProximaFoto - Time.time).ToString("F2") + "s");
        }
    }

    private void HandleToggleFlash()
    {
        toggleFlash = !toggleFlash;
        AudioManager.instance.Play3D("Flash" + (toggleFlash ? "On" : "Off"), transform.position);
        Debug.Log("Flash: " + (toggleFlash ? "ON" : "OFF"));
    }

    private void HandleSave()
    {
        if (fotoFisica == null)
        {
            Debug.LogWarning("No hay foto física para guardar.");
            return;
        }

        MeshRenderer meshRenderer = fotoFisica.GetComponentInChildren<MeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.LogWarning("La foto física no tiene MeshRenderer.");
            return;
        }

        Texture2D textura = meshRenderer.material.mainTexture as Texture2D;
        if (textura == null)
        {
            Debug.LogWarning("La foto física no tiene textura.");
            return;
        }

        GuardarFoto(fotoFisica, textura);
    }

    #endregion

    #region Flash Effect

    private void ResetearRotacionCameraMesh()
    {
        if (CameraMesh != null)
        {
            CameraMesh.transform.localRotation = Quaternion.identity;
            if (panTiltComponent != null)
            {
                panTiltComponent.PanAxis.Value = transform.eulerAngles.y;
                panTiltComponent.TiltAxis.Value = 0f;
            }
        }
        else
        {
            Debug.LogError("CameraMesh no asignada en el Inspector.");
        }
    }

    public void Flashing()
    {
        if (flashEffect != null) flashEffect.SetActive(true);
        if (luzFlash != null)
        {
            luzFlash.enabled = true;
            luzFlash.intensity = intensidadMaximaFlash;
        }
        StartCoroutine(SecuenciaDesactivarFlash());
    }

    private IEnumerator SecuenciaDesactivarFlash()
    {
        yield return new WaitForSeconds(flashDuration);
        if (flashEffect != null) flashEffect.SetActive(false);
        StartCoroutine(DesvanecerFlash());
    }

    private IEnumerator DesvanecerFlash()
    {
        if (luzFlash == null) yield break;

        float tiempoPasado = 0f;
        while (tiempoPasado < duracionApagadoFlash)
        {
            tiempoPasado += Time.deltaTime;
            luzFlash.intensity = Mathf.Lerp(intensidadMaximaFlash, 0f, tiempoPasado / duracionApagadoFlash);
            yield return null;
        }
        luzFlash.intensity = 0f;
        luzFlash.enabled = false;
    }

    #endregion

    #region Foto e Interacción

    private IEnumerator ProcesoTomarFoto()
    {
        Debug.Log("Iniciando captura...");
        yield return new WaitForEndOfFrame();

        Texture2D fotoCapturada = ScreenCapture.CaptureScreenshotAsTexture();
        List<GameObject> enemigosCapturados = detector.DetectarEnemigosEnFoto();

        if (fotoFisica != null)
        {
            GuardarFoto(fotoFisica, texturaFoto,contadorFotos--);
            fotoFisica = null;
        }

        if (prefabFotoFisica != null && puntoDeAparicion != null)
        {
            GameObject fotoInstanciada = Instantiate(prefabFotoFisica, puntoDeAparicion.position, puntoDeAparicion.rotation);
            fotoInstanciada.transform.SetParent(puntoDeAparicion);
            fotoFisica = fotoInstanciada;
            texturaFoto = fotoCapturada;
            
            MeshRenderer meshRenderer = fotoInstanciada.GetComponentInChildren<MeshRenderer>();
            if (meshRenderer != null)
            {
                meshRenderer.material.mainTexture = fotoCapturada;
                Debug.Log("Foto creada y material aplicado.");
            }
            else
            {
                Debug.LogError("El prefab de la foto no tiene MeshRenderer.");
            }
            DatosFotos nuevosDatosFoto = new DatosFotos("Foto_" + contadorFotos, "", reavelTime, enemigosCapturados);
            Item nuevoItem = new Item
            {
                name = "Foto_" + contadorFotos,
                description = "Una foto tomada el " + System.DateTime.Now.ToString("dd/MM/yyyy"),
                esFoto = true,
                datosFoto = nuevosDatosFoto,
                prefabItem = prefabFotoFisica
            };
            FotoRevelado scriptRevelado = fotoInstanciada.GetComponent<FotoRevelado>();
            if (scriptRevelado != null)
                scriptRevelado.datos = nuevosDatosFoto;
            else
                Debug.LogError("El prefab de la foto no tiene el script FotoRevelado.");

        }
        else
        {
            Debug.LogError("Falta el Prefab o el Punto de Aparición.");
        }
    }

    public void InteractuarConFoto(GameObject foto)
    {
        if (foto != null)
            inspectSystem.EnterInspectionMode(foto);
        else
            Debug.LogError("No se puede interactuar con una foto nula.");
    }

    public void GuardarFoto(GameObject foto, Texture2D texturaFoto, int? idFotos = null)
    {
        if (foto == null) { Debug.LogError("Foto nula."); return; }
        if (texturaFoto == null) { Debug.LogError("Textura nula."); return; }
        if (!idFotos.HasValue) idFotos = contadorFotos;
        string idFoto = "Foto_" + idFotos;
        string rutaCompleta = Path.Combine(rutaFotos, idFoto + ".png");

        if (!Directory.Exists(rutaFotos))
            Directory.CreateDirectory(rutaFotos);

        byte[] bytes = texturaFoto.EncodeToPNG();
        File.WriteAllBytes(rutaCompleta, bytes);

        float progresoRescatado = 0f;
        List<GameObject> enemigosRescatados = new List<GameObject>(); // Lista vacía por defecto

        FotoRevelado scriptRevelado = foto.GetComponent<FotoRevelado>();
        if (scriptRevelado != null && scriptRevelado.datos != null)
        {
            progresoRescatado = scriptRevelado.datos.revealProgress;
            // ¡Recuperamos la lista de enemigos que guardamos en ProcesoTomarFoto!
            enemigosRescatados = scriptRevelado.datos.enemigosCapturados; 
        }

        // 4. CREAR EL NUEVO OBJETO DATOSFOTOS INCLUYENDO LA LISTA RESCATADA
        DatosFotos nuevaFoto = new DatosFotos(idFoto, rutaCompleta, reavelTime, enemigosRescatados)
        {
            revealProgress = progresoRescatado
        };

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
            Debug.Log("Foto guardada: " + rutaCompleta);
        else
            Debug.LogWarning("No se pudo agregar al inventario.");

        Destroy(texturaFoto);
        Destroy(foto);
    }

    /*private void DetectarYEliminarEnemigos()
    {
        List<GameObject> enemigosCapturados = detector.DetectarEnemigosEnFoto();

        foreach (GameObject enemigo in enemigosCapturados)
        {
            // Notificar al enemigo que fue fotografiado
            var flashEnemy = enemigo.gameObject.GetComponent<FlashEnemy>();
            if (flashEnemy != null)
                flashEnemy.OnFotografiado(enemigo.prominencia);

            // Calcular puntuación según prominencia y distancia
            float puntos = CalcularPuntuacion(enemigo);
            Debug.Log($"{enemigo.gameObject.name} fotografiado — {puntos} pts");
        }

        // Guardar en DatosFoto qué enemigos salían
        nuevaFoto.enemigosCapturados = enemigosCapturados
            .Select(e => e.gameObject.name)
            .ToList();
    }*/

    #endregion

    private void OnDrawGizmosSelected()
    {
        if (aimCamera == null) return;
        Gizmos.color = Color.cyan;
        Vector3 centro = aimCamera.transform.position + aimCamera.transform.forward * (sizeZonaFoto.z / 2f);
        Gizmos.matrix = Matrix4x4.TRS(centro, aimCamera.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, sizeZonaFoto);
        Gizmos.matrix = Matrix4x4.identity;
    }
}