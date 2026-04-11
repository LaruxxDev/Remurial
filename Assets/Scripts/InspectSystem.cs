using UnityEngine;
using UnityEngine.InputSystem;

public class InspectSystem : MonoBehaviour
{
    #region Values
    [Header("References")]
    public Transform AparitionPoint; // Punto donde el objeto aparecerá al inspeccionarlo
    public GameObject rawImageUI; // UI para mostrar la imagen del objeto inspeccionado

    [Header("Input System")]
    public InputActionAsset inputAsset; // Referencia al componente InputAsset para manejar las acciones de entrada
    public InputActionReference accionClick; // Acción para rotar el objeto inspeccionado

    public InputActionReference accionRotar; // Acción para rotar el objeto inspeccionado
    public InputActionReference accionRotarMando; // Acción para rotar el objeto inspeccionado

    public InputActionReference accionSalir; // Acción para salir del modo de inspección


    [Header("Settings")]
    public float rotationSpeed = 10f; // Velocidad de rotación del objeto inspeccionado

    private GameObject currentInspectedObject; // Objeto actualmente inspeccionado
    private bool isInspecting = false; // Indica si el jugador está inspeccionando un objeto
    #endregion

    
    #region Unity Methods
    private void Update()
    {
        if (isInspecting && currentInspectedObject != null && (accionClick.action.IsPressed() || accionRotarMando.action.IsPressed()))
        {
            // Permitir rotar el objeto inspeccionado con el mouse
            Vector2 rotationInput = accionRotar.action.ReadValue<Vector2>();

            if (rotationInput != Vector2.zero)
            {
                float rotationX = rotationInput.x * rotationSpeed * Time.deltaTime;
                float rotationY = rotationInput.y * rotationSpeed * Time.deltaTime;

                currentInspectedObject.transform.Rotate(Vector3.up, -rotationX, Space.World);
                currentInspectedObject.transform.Rotate(Vector3.right, -rotationY, Space.World);
            }
        }

    if (isInspecting && accionSalir.action.WasPressedThisFrame())
        {
            ExitInspectionMode();
        }
    }
    #endregion
    
    public void EnterInspectionMode(GameObject objectToInspect)
    {
        if (isInspecting) return; // Evitar entrar en modo inspección si ya se está inspeccionando algo
        Debug.Log("Entrando en modo inspección con el objeto: " + objectToInspect.name);
        currentInspectedObject = Instantiate(objectToInspect, AparitionPoint.position, Quaternion.identity);
        FotoRevelado reveladorNuevo = currentInspectedObject.GetComponent<FotoRevelado>();
        if (reveladorNuevo != null)
        {
            reveladorNuevo.isInspecting = true;
            reveladorNuevo.RevelarInstantaneo();
        }
        MeshRenderer mrOriginal = objectToInspect.GetComponentInChildren<MeshRenderer>();
        MeshRenderer mrNuevo = currentInspectedObject.GetComponentInChildren<MeshRenderer>();
        if (mrOriginal != null && mrNuevo != null)
        {
            mrNuevo.material = new Material(mrOriginal.material);
            mrNuevo.material.mainTexture = mrOriginal.material.mainTexture;
        }
        rawImageUI.SetActive(true); // Mostrar la UI de inspección
        isInspecting = true;

        inputAsset.FindActionMap("Player").Disable();
        inputAsset.FindActionMap("UI").Enable(); // Cambiar al mapa de acciones de UI para manejar la rotación y salida
    }

    public void ExitInspectionMode()
    {
        if (!isInspecting) return; // Evitar salir del modo inspección si no se está inspeccionando nada

        Destroy(currentInspectedObject); // Eliminar el objeto inspeccionado
        rawImageUI.SetActive(false); // Ocultar la UI de inspección
        isInspecting = false;

        inputAsset.FindActionMap("UI").Disable();
        inputAsset.FindActionMap("Player").Enable(); // Volver al mapa de acciones del jugador
    }
}
