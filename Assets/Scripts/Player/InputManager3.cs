using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager3 : MonoBehaviour
{
    [Header("Input Actions")]
    [SerializeField] private InputActionReference moveAction; 
    [SerializeField] private InputActionReference attackAction; 
    [SerializeField] private InputActionReference interactAction; 

    [Header("Movement Settings")]
    private Rigidbody rb;
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f;
    public Vector2 movementInput { get; private set; }


    [Header("Interaction")]
    public InspectSystem inspectSystem; 
    private GameObject interactuableItem; // Objeto interacutuable que el jugador puede recoger



    public AnimatorManager animatorManager;


    private void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }
    
    private void Update()
    {
        movementInput = moveAction.action.ReadValue<Vector2>();
        animatorManager.HandleAnimatorValues(movementInput.x, movementInput.y);


        if (interactAction.action.WasPressedThisFrame() && interactuableItem != null)
        {
            
            // animatorManager.PlayInteractAnimation();
            inspectSystem.EnterInspectionMode(interactuableItem); // Entrar en modo inspección con el objeto interactuable
            Destroy(interactuableItem); // Destruir el objeto interactuable después de usarlo (opcional)
            interactuableItem = null; // Limpiar la referencia al objeto interactuable después de usarlo
            
        }

    }

    private void FixedUpdate()
    {
        // 1. ROTACIÓN TIPO TANQUE (Eje X del input)
        // Calculamos los grados a rotar basados en la velocidad y el tiempo de físicas
        float turn = movementInput.x * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        
        // Aplicamos la rotación al Rigidbody
        rb.MoveRotation(rb.rotation * turnRotation);

        // 2. MOVIMIENTO ADELANTE/ATRÁS (Eje Y del input)
        // Usamos 'transform.forward' para que siempre avance hacia donde está mirando
        Vector3 moveDirection = transform.forward * movementInput.y * moveSpeed;

        // Aplicamos la velocidad, respetando la gravedad en el eje Y
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }
    
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Interactuable"))
        {
            interactuableItem = other.gameObject; // Guardamos el objeto interacutuable para usarlo al interactuar
            Debug.Log("Detectado objeto: " + other.name);
            if (HudManager.Instance != null)
            {
                HudManager.Instance.MostrarMensaje("Pulsa [E] para inspeccionar " + other.name);
            }
            else 
            {
                Debug.LogWarning("No se encontró el HUDManager en la escena.");
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Interactuable") && interactuableItem == other.gameObject)
        {
            interactuableItem = null; // Limpiamos la referencia al salir del área de interacción
        }
    }


}

