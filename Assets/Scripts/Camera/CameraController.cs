using Unity.Cinemachine;
using UnityEngine;
using System.IO;
using System.Collections;

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

    private int contadorFotos = 0;
    private float tiempoProximaFoto = 0f;
    private bool toggleFlash = true;
    private GameObject fotoFisica = null;

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
        if (panTiltComponent != null)
        {
            panTiltComponent.PanAxis.Value = 0f;
            panTiltComponent.TiltAxis.Value = 0f;
        }
        aimCamera.Priority = defaultPriority;
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
                panTiltComponent.PanAxis.Value = 0f;
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
        DetectarYEliminarEnemigos();

        if (fotoFisica != null)
        {
            GuardarFoto(fotoFisica, fotoCapturada);
            fotoFisica = null;
        }

        if (prefabFotoFisica != null && puntoDeAparicion != null)
        {
            GameObject fotoInstanciada = Instantiate(prefabFotoFisica, puntoDeAparicion.position, puntoDeAparicion.rotation);
            fotoInstanciada.transform.SetParent(puntoDeAparicion);
            fotoFisica = fotoInstanciada;

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

    public void GuardarFoto(GameObject foto, Texture2D texturaFoto)
    {
        if (foto == null) { Debug.LogError("Foto nula."); return; }
        if (texturaFoto == null) { Debug.LogError("Textura nula."); return; }

        string idFoto = "Foto_" + contadorFotos;
        string rutaCompleta = Path.Combine(rutaFotos, idFoto + ".png");

        if (!Directory.Exists(rutaFotos))
            Directory.CreateDirectory(rutaFotos);

        byte[] bytes = texturaFoto.EncodeToPNG();
        File.WriteAllBytes(rutaCompleta, bytes);

        float progresoRescatado = 0f;
        FotoRevelado scriptRevelado = foto.GetComponent<FotoRevelado>();
        if (scriptRevelado != null && scriptRevelado.datos != null)
            progresoRescatado = scriptRevelado.datos.revealProgress;

        DatosFotos nuevaFoto = new DatosFotos(idFoto, rutaCompleta, reavelTime);
        nuevaFoto.revealProgress = progresoRescatado;

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

    private void DetectarYEliminarEnemigos()
    {
        Vector3 centro = aimCamera.transform.position + aimCamera.transform.forward * (sizeZonaFoto.z / 2f);
        Quaternion rotacion = aimCamera.transform.rotation;

        Collider[] objetosDetectados = Physics.OverlapBox(centro, sizeZonaFoto / 2f, rotacion, capaEnemigos);

        if (objetosDetectados.Length > 0)
        {
            foreach (Collider col in objetosDetectados)
            {
                if (col.CompareTag("Enemy"))
                {
                    EnemyCollision enemyCollision = col.GetComponent<EnemyCollision>();
                    if (enemyCollision != null) enemyCollision.FOTOMADE = true;
                    Debug.Log($"Enemigo fotografiado: {col.gameObject.name}");
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
        Gizmos.matrix = Matrix4x4.TRS(centro, aimCamera.transform.rotation, Vector3.one);
        Gizmos.DrawWireCube(Vector3.zero, sizeZonaFoto);
        Gizmos.matrix = Matrix4x4.identity;
    }
}