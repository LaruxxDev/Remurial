using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMove : MonoBehaviour
{
    [Header("Input System")]
    [SerializeField] private GameInputReader _input; // Referencia a tu ScriptableObject

    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    public float rotationSpeed = 120f;

    [Header("References")]
    public AnimatorManager animatorManager;
    private Rigidbody rb;

    // Guardamos el input actual aquí
    private Vector2 movementInput;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();    
    }

    // ─── SUSCRIPCIÓN A EVENTOS ──────────────────────────────────────────

    private void OnEnable()
    {
        if (_input != null)
        {
            // Nos suscribimos al evento de movimiento de tu GameInputReader
            _input.OnMoveEvent += HandleMove;
        }
    }

    private void OnDisable()
    {
        if (_input != null)
        {
            // Es vital desuscribirse para evitar errores si el jugador es destruido
            _input.OnMoveEvent -= HandleMove;
        }
    }

    // ─── LÓGICA DE MOVIMIENTO ───────────────────────────────────────────

    // Este método se llamará automáticamente CADA VEZ que el input cambie
    private void HandleMove(Vector2 input)
    {
        movementInput = input;
        
        // Actualizamos las animaciones solo cuando cambia el input
        if (animatorManager != null)
        {
            animatorManager.HandleAnimatorValues(movementInput.x, movementInput.y);
        }
    }

    private void FixedUpdate()
    {
        // 1. ROTACIÓN TIPO TANQUE (Eje X del input)
        float turn = movementInput.x * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        rb.MoveRotation(rb.rotation * turnRotation);

        // 2. MOVIMIENTO ADELANTE/ATRÁS (Eje Y del input)
        Vector3 moveDirection = transform.forward * movementInput.y * moveSpeed;
        
        // Aplicamos la velocidad, respetando la gravedad en el eje Y
        rb.linearVelocity = new Vector3(moveDirection.x, rb.linearVelocity.y, moveDirection.z);
    }
}