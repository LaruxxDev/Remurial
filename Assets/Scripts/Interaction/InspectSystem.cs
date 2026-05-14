using UnityEngine;

public class InspectSystem : MonoBehaviour
{
    #region Values
    [Header("References")]
    public Transform AparitionPoint;
    public GameObject rawImageUI; 

    [Header("Input System")]
    [SerializeField] private GameInputReader _input; 

    [Header("Settings")]
    public float rotationSpeed = 10f; 

    private GameObject currentInspectedObject; 
    private bool isInspecting = false; 
    
    // Variables para almacenar el estado de los inputs
    private bool isHoldingClick = false;
    private Vector2 mouseRotationInput;
    private Vector2 mandoRotationInput;
    #endregion
    
    #region Unity Methods
    private void OnEnable()
    {
        // Nos suscribimos a los eventos de UI
        _input.OnClickEvent += HandleClick;
        _input.OnRotateEvent += HandleRotation;
        _input.OnRotateMandoEvent += HandleMandoRotation;
        _input.OnCancelUIEvent += HandleCancel;
    }

    private void OnDisable()
    {
        // IMPORTANTE: Desuscribirse de todos para evitar fugas de memoria
        _input.OnClickEvent -= HandleClick;
        _input.OnRotateEvent -= HandleRotation;
        _input.OnRotateMandoEvent -= HandleMandoRotation; 
        _input.OnCancelUIEvent -= HandleCancel;
    }

    private void Update()
    {
        if (isInspecting && currentInspectedObject != null)
        {
            // Empezamos con un vector a cero
            Vector2 finalRotation = Vector2.zero;

            // 1. La rotación del mando se aplica SIEMPRE (no requiere click)
            finalRotation += mandoRotationInput;

            // 2. La rotación del ratón se aplica SOLO si se mantiene el click
            if (isHoldingClick)
            {
                finalRotation += mouseRotationInput;
            }

            // Si hay alguna rotación (ya sea de mando o de ratón con click)
            if (finalRotation != Vector2.zero)
            {
                float rotationX = finalRotation.x * rotationSpeed * Time.unscaledDeltaTime;
                float rotationY = finalRotation.y * rotationSpeed * Time.unscaledDeltaTime;

                currentInspectedObject.transform.Rotate(Vector3.up, -rotationX, Space.World);
                currentInspectedObject.transform.Rotate(Vector3.right, -rotationY, Space.World);
            }
        }
    }
    #endregion

    #region Input Callbacks
    private void HandleClick(bool isPressed)
    {
        isHoldingClick = isPressed;
    }
    
    private void HandleMandoRotation(Vector2 rotationInput)
    {
        mandoRotationInput = rotationInput;
    }
    
    private void HandleRotation(Vector2 rotationInput)
    {
        mouseRotationInput = rotationInput;
    }

    private void HandleCancel()
    {
        if (isInspecting)
        {
            ExitInspectionMode();
        }
    }
    #endregion
    
    public void EnterInspectionMode(GameObject objectToInspect)
    {
        InventarioManager.Instance.isIspecting = true; // Informamos al inventario que estamos inspeccionando
        if (isInspecting) return; 
        GameManager.Instancia.TogglePausa(); // Pausamos el juego al entrar en modo inspección
        Debug.Log("Entrando en modo inspección con el objeto: " + objectToInspect.name);
        currentInspectedObject = Instantiate(objectToInspect, AparitionPoint.position, Quaternion.identity);
        
        FotoRevelado reveladorNuevo = currentInspectedObject.GetComponent<FotoRevelado>();
        if (reveladorNuevo != null)
        {
            reveladorNuevo.isInspecting = true;
            Debug.Log("Progreso rescatado: " + reveladorNuevo.datos.revealProgress);
        }
        
        MeshRenderer mrOriginal = objectToInspect.GetComponentInChildren<MeshRenderer>();
        MeshRenderer mrNuevo = currentInspectedObject.GetComponentInChildren<MeshRenderer>();
        if (mrOriginal != null && mrNuevo != null)
        {
            mrNuevo.material = new Material(mrOriginal.material);
            mrNuevo.material.mainTexture = mrOriginal.material.mainTexture;
        }
        
        rawImageUI.SetActive(true); 
        isInspecting = true;

        // Cambiamos de contexto
        _input.DisableAll();
        _input.EnableUI(); // Descomentado: Necesario para que HandleClick, HandleRotation, etc., se disparen
    }

    public void ExitInspectionMode()
    {
        InventarioManager.Instance.isIspecting = false; // Informamos al inventario que ya no estamos inspeccionando
        if (!isInspecting) return; 
        GameManager.Instancia.TogglePausa(); // DesPausamos el juego al entrar en modo inspección

        Destroy(currentInspectedObject); 
        rawImageUI.SetActive(false); 
        isInspecting = false;
        
        // Reiniciamos TODOS los valores por seguridad
        isHoldingClick = false;
        mouseRotationInput = Vector2.zero;
        mandoRotationInput = Vector2.zero;

        // Volvemos al control del jugador
        _input.EnableGameplay(); 
    }
}