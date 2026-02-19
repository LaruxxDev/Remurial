using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private InputActionReference moveAction; 
    private Rigidbody rb;
    public float moveSpeed = 5f;
    public float rotationSpeed = 10f;

    public Vector2 movementInput { get; private set; }

    private AnimatorManager animatorManager;
    private Transform cameraTransform;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
        cameraTransform = Camera.main.transform;
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
        Vector3 moveDirection = new Vector3(movementInput.x, 0, movementInput.y);
        moveDirection = cameraTransform.TransformDirection(moveDirection);
        moveDirection.y = 0;

        rb.linearVelocity = new Vector3(moveDirection.x * moveSpeed, rb.linearVelocity.y, moveDirection.z * moveSpeed);

        if (moveDirection != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            rb.MoveRotation(Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * rotationSpeed));
        }
    }
}