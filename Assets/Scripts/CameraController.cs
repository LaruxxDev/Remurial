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


    [Header("Configuración")]
    //[SerializeField] private string rutaFotos = "Assets/Fotos/"; // Ruta para guardar las fotos
    public int aimPriority = 20; // Prioridad alta al apuntar
    public int defaultPriority = 9; // Prioridad baja al dejar de apuntar
    public int coldownFoto = 2; // Tiempo de espera entre cada foto (en segundos)


    [Header("Configuración del Prefab")]
    [SerializeField] private GameObject prefabFotoFisica; 
    [SerializeField] private Transform puntoDeAparicion;

    [Header("Interaction")]
    public InspectSystem inspectSystem; 

    [Header("Configuración de Flash")]
    [SerializeField] private float flashDuration = 3.0f; // Duración del flash en segundos
    [SerializeField] private GameObject flashEffect; // Efecto visual para el flash 
    [SerializeField] private Light luzFlash; // Luz para el efecto de flash
    [SerializeField] private float intensidadMaximaFlash = 122f; // Intensidad máxima de la luz durante el flash
    [SerializeField] private float duracionApagadoFlash = 1f; // Tiempo que tarda en apagarse la luz después del flash


    private int contadorFotos = 0;
    private float tiempoProximaFoto = 0f;

    #endregion
    #region Unity Methods
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
            Flashing();
            StartCoroutine(ProcesoTomarFoto());
        }
        else
        {
            if (attackAction.action.WasPressedThisFrame() && aimCamera.Priority != aimPriority)
            {
                Debug.Log("No se puede tomar foto: No estás apuntando.");
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

            // Buscamos el MeshRenderer 
            MeshRenderer quadRenderer = fotoInstanciada.GetComponentInChildren<MeshRenderer>();

            if (quadRenderer != null)
            {
                // Le aplicamos el nuevo material al Quad
                quadRenderer.material.mainTexture = fotoCapturada;
                Debug.Log("¡Foto creada físicamente y material aplicado al Quad!");
                InteractuarConFoto(fotoInstanciada);
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
    #endregion
}