using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


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
            TomarFoto();
        }
    }

    void TomarFoto()
    {
        string nombreArchivo = "Captura_" + System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss") + ".png";
        string rutaCompleta = rutaFotos + nombreArchivo;
        ScreenCapture.CaptureScreenshot(rutaCompleta);
        Debug.Log("¡Flash! Foto tomada.");
        Debug.Log("Foto guardada en: " + rutaCompleta);


        byte[] bytes = File.ReadAllBytes(rutaCompleta);
        Texture2D tex = new Texture2D(2, 2, TextureFormat.RGBA32, false);
        if (tex.LoadImage(bytes))
        {
            Material mat = new Material(Shader.Find("Standard"));
            mat.mainTexture = tex;
            if (targetRenderer != null) targetRenderer.material = mat;
            Debug.Log("Material creado y aplicado con la foto.");
        }
        else Debug.LogError("No se pudo cargar imagen");
        yield return null;
    }
}