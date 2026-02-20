using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction; 
    private Rigidbody rb;
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f;

    public Vector2 movementInput { get; private set; }

    private AnimatorManager animatorManager;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }
    
    private void Update()
    {
        movementInput = moveAction.action.ReadValue<Vector2>();
        animatorManager.HandleAnimatorValues(movementInput.x, movementInput.y);
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
}