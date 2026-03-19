using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using System.IO;
using System.Collections;

public class CameraController : MonoBehaviour
{
    [Header("Cámaras")]
    [SerializeField] private CinemachineCamera aimCamera; 
    [SerializeField] private GameObject CameraMesh; 

    private CinemachinePanTilt panTiltComponent;

    [Header("Input Actions")]
    [SerializeField] private InputActionReference attackAction; 
    [SerializeField] private InputActionReference aimAction; 


    [Header("Configuración")]
    [SerializeField] private string rutaFotos = "Assets/Fotos/"; // Ruta para guardar las fotos
    public int aimPriority = 20; // Prioridad alta al apuntar
    public int defaultPriority = 9; // Prioridad baja al dejar de apuntar


    [Header("Configuración del Prefab")]
    [SerializeField] private GameObject prefabFotoFisica; 
    [SerializeField] private Transform puntoDeAparicion;

    [Header("Interaction")]
    public InspectSystem inspectSystem; 
    private GameObject interactuableItem;

    [Header("Configuración de Material")]
    [SerializeField] private Material materialBase;
    private int contadorFotos = 0;

    void Start()
    {
        if (aimCamera != null)
        {
            panTiltComponent = aimCamera.GetComponent<CinemachinePanTilt>();
        }
    }

    void Update()
    {
        if (aimAction.action.WasPressedThisFrame())
        {
            aimCamera.Priority = aimPriority;
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
        
        if (attackAction.action.WasPressedThisFrame() && aimCamera.Priority == aimPriority)
        {
            StartCoroutine(ProcesoTomarFoto());
        }
    }

    private IEnumerator ProcesoTomarFoto()
    {
        Debug.Log("¡Flash! Iniciando captura...");
        contadorFotos++; // Incrementamos el ID
        string nombreArchivo = "Captura_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss-fff") + "_" + contadorFotos + ".png";

        string rutaCompleta = rutaFotos + nombreArchivo;

        ScreenCapture.CaptureScreenshot(rutaCompleta);

        Debug.Log("¡Flash! Foto tomada.");

        Debug.Log("Foto guardada en: " + rutaCompleta); 
        // 1. ESPERA CRÍTICA: Debemos esperar a que Unity termine de dibujar todo este frame
        yield return new WaitForEndOfFrame();
        Material materialDeLaFoto = CrearMaterialDesdePNG(rutaCompleta);

        // 4. INSTANCIACIÓN Y ASIGNACIÓN (Tu pregunta principal)
        if (prefabFotoFisica != null && puntoDeAparicion != null)
        {
            // Creamos el prefab en la posición y rotación del punto de aparición
            GameObject fotoInstanciada = Instantiate(prefabFotoFisica, puntoDeAparicion.position, puntoDeAparicion.rotation);

            // Buscamos el MeshRenderer del Quad. Asumimos que es el objeto mismo o un hijo.
            // Es vital que tu Prefab tenga un Quad con MeshRenderer.
            MeshRenderer quadRenderer = fotoInstanciada.GetComponentInChildren<MeshRenderer>();

            if (quadRenderer != null)
            {
                // ¡Le aplicamos el nuevo material al Quad!
                quadRenderer.material = materialDeLaFoto;
                Debug.Log("¡Foto creada físicamente y material aplicado al Quad!");
            }
            else
            {
                Debug.LogError("El prefab de la foto no tiene un MeshRenderer (Quad) en sí mismo o sus hijos.");
            }
        }
        else
        {
            Debug.LogError("Falta asignar el Prefab de la foto o el Punto de Aparición en el Inspector.");
        }
    }



    public Material CrearMaterialDesdePNG(string rutaArchivo)
    {
        if (!File.Exists(rutaArchivo)) return null;

        byte[] datosArchivo = File.ReadAllBytes(rutaArchivo);
        Texture2D textura = new Texture2D(2, 2);
        
        if (textura.LoadImage(datosArchivo))
        {
            // Creamos una COPIA del material base para no modificar el original de la carpeta Assets
            Material materialNuevo = new Material(materialBase);
            
            // Asignamos la textura. 
            // Esto funciona para la mayoría de los Shaders de Unity
            materialNuevo.mainTexture = textura; 
            
            // Si usas URP y lo anterior falla, descomenta esta línea:
            // materialNuevo.SetTexture("_BaseMap", textura);

            return materialNuevo;
        }
        return null;
    }    
}