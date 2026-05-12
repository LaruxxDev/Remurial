using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Collections;
using UnityEditor.Timeline.Actions;

public class PhotoController : MonoBehaviour
{
    #region Values and References
    [Header("Configuración")]
    public float revealTime = 5f;                   // Tiempo que tarda en revelarse la foto
    private string folderRoute;    // Ruta donde se guardarán las fotos
    [SerializeField] private PlayerGeneral PLAYER;

    GameObject photoObject = null;


    [Header("Camera")]
    [SerializeField] private CinemachineCamera aimCamera;
    [SerializeField] private GameObject CameraMesh;

    [SerializeField] private CinemachinePanTilt panTiltComponent;


    [Header("Flash")]
    [SerializeField] private GameObject photoArea;      // Area de la foto
    [SerializeField] private GameObject flashArea;      // Area del flash 
    [SerializeField] private Light flashLight;          // Luz del flash

    [SerializeField] private bool toggleFlash;          // Alternar el flash
    [SerializeField] private float flashDuration;       // Duración del flash en segundos
    [SerializeField] private float flashMaxIntensity;   // Intensidad máxima de la luz durante el flash


    [Header("Photo Prefab")]
    [SerializeField] private GameObject photoPrefab;    // Prefab de la foto
    [SerializeField] private Transform photoSpawnPoint; // Zona de creación

    private int photoCount = 0; //GestorInventario.Instance.fotosEnInventario.Count; // Contador para nombrar las fotos de forma única


    [Header("Aim Values")]
    [SerializeField] private float yawLimit;            // Min/Max del yaw
    private float yawCurrent;

    [SerializeField] private float pitchLimit;          // Min/Max del pitch
    private float pitchCurrent;

    [SerializeField] private ItemDefinition photoDefinition;
    #endregion


    // CUANDO EXPORTEMOS LA CARPETA ASSETS NO SALDRÁN LOS ARCHIVOS DE FOTOS, POR ESO GUARDAMOS EN PERSISTENTDATA
    void Awake()
    {
        // Guardaremos las fotos en una subcarpeta llamada "AlbumPolaroid"
        folderRoute = Path.Combine(Application.persistentDataPath, "AlbumPolaroid");

        if (!Directory.Exists(folderRoute))
            Directory.CreateDirectory(folderRoute);

        photoCount = Directory.GetFiles(folderRoute, "*.png").Length;
    }

    void Start()
    {
        if (aimCamera != null)
            panTiltComponent = aimCamera.GetComponent<CinemachinePanTilt>();


        flashMaxIntensity = flashLight.intensity;
    }


    // Update solo llamado cuando el estado correcto está activo
    public void CustomUpdate()
    {
        Vector2 look = PLAYER.INPUTTRANSFORMER.INPUTAIMNORMAL;

        float yaw = look.x * PLAYER.CONFIGURATION.SENSITIVITY;
        float pitch = look.y * PLAYER.CONFIGURATION.SENSITIVITY;

        ApplyLook(yaw, pitch);
    }

    #region Aim
    private void ApplyLook(float yaw, float pitch)
    {
        if (panTiltComponent == null)
            return;

        // Yaw
        yawCurrent += yaw;
        yawCurrent = Mathf.Clamp(yawCurrent, -yawLimit, yawLimit);

        // Pitch
        pitchCurrent -= pitch;
        pitchCurrent = Mathf.Clamp(pitchCurrent, -pitchLimit, pitchLimit);

        // Set
        panTiltComponent.PanAxis.Value = yawCurrent;
        panTiltComponent.TiltAxis.Value = pitchCurrent;
    }    
    
    // Resetear Valores al salir de la cámara
    public void ResetCamera()
    {
        if (aimCamera == null)
            return;

        // Rotación
        aimCamera.transform.localRotation = Quaternion.identity;

        // Modo Cámara
        if (panTiltComponent != null)
        {
            panTiltComponent.PanAxis.Value = 0f;
            panTiltComponent.TiltAxis.Value = 0f;

            pitchCurrent = 0f;
            yawCurrent = 0f;
        }   
    }
    #endregion

    #region Photo
    public void TakePhoto()
    {
        if (photoPrefab == null || photoSpawnPoint == null)
            return;

        if (!PLAYER.CONFIGURATION.CanUseCamera())
            return;

        PLAYER.CONFIGURATION.ResetCamera();

        photoCount++;

        // Flash
        if (toggleFlash)
            Flashing();


        StartCoroutine(TakePhotoProcess());
    }

    // Corrutina. Toma la foto
    private IEnumerator TakePhotoProcess()
    {
        if (photoObject != null)
            Destroy(photoObject);

        // Detección de enemigos
        photoArea.SetActive(true);
        yield return new WaitForEndOfFrame();

        // Screenshot
        Texture2D fotoCapturada = ScreenCapture.CaptureScreenshotAsTexture();

        // Pequeño delay
        yield return null;
        photoArea.SetActive(false);


        // Instanciado de foto y asignación
        if (photoPrefab != null && photoSpawnPoint != null)
        {
            // Creamos el prefab en la posición y rotación del punto de aparición
            GameObject fotoInstanciada = Instantiate(photoPrefab, photoSpawnPoint.position, photoSpawnPoint.rotation);
            fotoInstanciada.transform.SetParent(photoSpawnPoint);

            fotoInstanciada.transform.localPosition = new Vector3(0f, 0f, 0f);
            fotoInstanciada.transform.localScale = new Vector3(0.3f, 0.3f, 0.015f);

            // Buscamos el MeshRenderer 
            MeshRenderer meshRenderer = fotoInstanciada.GetComponentInChildren<MeshRenderer>();

            PhotoEnemy enemiesCaughtScript = photoArea.GetComponent<PhotoEnemy>();

            // Identificar foto y ubicación
            string idFoto = "Foto_" + photoCount;
            string rutaCompleta = Path.Combine(folderRoute, idFoto + ".png");

            // Crear los datos con la ruta correcta y la textura ya cargada en memoria
            DatosFotos nuevaFoto = new DatosFotos(idFoto, rutaCompleta, revealTime, enemiesCaughtScript.GetEnemiesCaught());

            fotoInstanciada.GetComponent<RevealPhoto>().datos = nuevaFoto;


            if (meshRenderer != null)
            {
                // Le aplicamos el nuevo material al Quad
                meshRenderer.material.mainTexture = fotoCapturada;
                Debug.Log("¡Foto creada físicamente y material aplicado al Quad!");

                // Guardarla
                SavePhoto(fotoInstanciada, fotoCapturada, nuevaFoto, idFoto, rutaCompleta);

                photoObject = fotoInstanciada;
            }


            yield return null;
            enemiesCaughtScript.ClearList();
        }
    }

    // Guarda la foto en memoria/inventario
    public void SavePhoto(GameObject foto, Texture2D texturaFoto, DatosFotos data, string idFoto, string rutaCompleta)
    {
        if (foto == null || texturaFoto == null)
            return;

        // Asegurarse de que la carpeta exista
        if (!Directory.Exists(folderRoute))
            Directory.CreateDirectory(folderRoute);


        // Generar y guardar el PNG en disco
        byte[] bytes = texturaFoto.EncodeToPNG();
        File.WriteAllBytes(rutaCompleta, bytes);


        Item itemFoto = new Item(photoDefinition, 1);
        itemFoto.datosFoto = data;
        itemFoto.customName = idFoto;
        itemFoto.customDescription = "Una foto tomada el " + System.DateTime.Now.ToString("dd/MM/yyyy");

        bool agregada = InventoryManager.Instance.AgregarItem(itemFoto);

        if (!agregada)
            Debug.LogWarning("No se pudo agregar al inventario (¿lleno?).");


        // Destruir la foto física del mundo después de guardarla
        Destroy(foto);

        Debug.Log("Foto guardada y añadida al inventario: " + rutaCompleta);
    }
    #endregion

    #region Flash
    //Alternar el flash
    public void ToggleFlash()
    {
        toggleFlash = !toggleFlash;
    }

    // Empezar el flash
    public void Flashing()
    {
        if (flashArea != null)
            flashArea.SetActive(true);

        StartCoroutine(FlashRoutine());
    }

    // Secuencia de encendido y apagado del flash
    private IEnumerator FlashRoutine()
    {
        if (flashLight == null) yield break;

        float tiempoPasado = 0f;

        // Mientras el tiempo que ha pasado sea menor a la duración que queremos...
        while (tiempoPasado < flashDuration)
        {
            tiempoPasado += Time.deltaTime;

            // Lerp mezcla dos valores. Va de intensidadMaximaFlash a 0 a lo largo del tiempo.
            flashLight.intensity = Mathf.Lerp(flashMaxIntensity, 0f, tiempoPasado / flashDuration);

            // Esperamos al siguiente frame para seguir bajando la intensidad
            yield return null;
        }

        // Pasado ese tiempo, apagamos el efecto visual de golpe
        if (flashArea != null)
            flashArea.SetActive(false);

        // Potencia reseteada
        flashLight.intensity = flashMaxIntensity;
    }
    #endregion
}