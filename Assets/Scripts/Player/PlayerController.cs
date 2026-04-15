using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Idle, Walking, TakingPhoto, Revealing, Interacting }
    private PlayerState _currentState = PlayerState.Idle;

    [Header("Input")]
    [SerializeField] private GameInputReader _input;

    [Header("Movimiento")]
    public float moveSpeed = 3f;
    public float rotationSpeed = 120f;

    [Header("Vida")]
    public int health = 6;
    public  int maxHealth = 6;
    public int healthRegen = 1;

    [Header("Referencias")]
    public AnimatorManager animatorManager;
    [SerializeField] private Animator _animator;
    private Rigidbody _rb;
    private Vector2 _smoothMoveInput;

    private Vector2 _moveInput;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _input.OnMoveEvent         += HandleMove;
        _input.OnAttackStarted     += HandleTakePhoto;
        _input.OnRevealUp          += HandleReveal;
        _input.OnInteractPerformed += HandleInteract;
    }

    private void OnDisable()
    {
        _input.OnMoveEvent         -= HandleMove;
        _input.OnAttackStarted     -= HandleTakePhoto;
        _input.OnRevealUp          -= HandleReveal;
        _input.OnInteractPerformed -= HandleInteract;
    }

    private void FixedUpdate()
    {
        if (_currentState == PlayerState.Interacting) return;
        MoverPersonaje();
    }

    // ── Input Handlers ───────────────────────────────────────

    private void HandleMove(Vector2 dir)
    {
        if (_currentState == PlayerState.Interacting) return;

        _moveInput = dir;
        _currentState = dir != Vector2.zero ? PlayerState.Walking : PlayerState.Idle;

    }

    private void HandleTakePhoto()
    {
        if (_currentState == PlayerState.Interacting) return;

        _currentState = PlayerState.TakingPhoto;
        _animator.SetTrigger("TakePhoto");
    }

    private void HandleReveal()
    {
        if (_currentState == PlayerState.Interacting) return;

        _currentState = PlayerState.Revealing;
        _animator.SetTrigger("Reveal");
    }

    private void HandleInteract()
    {
        _currentState = PlayerState.Interacting;
        _moveInput = Vector2.zero;

        if (animatorManager != null)
            animatorManager.HandleAnimatorValues(0f, 0f);

        _animator.SetTrigger("Interact");
    }

    // Llama desde Animation Event al terminar animaciones de acción
    public void OnActionFinished()
    {
        _currentState = PlayerState.Idle;
    }

    // ── Movimiento tipo tanque con Rigidbody ─────────────────
    private void Update()
    {
        if (_currentState == PlayerState.Interacting) return;
        
        _smoothMoveInput = Vector2.Lerp(_smoothMoveInput, _moveInput, Time.deltaTime * 10f);
        
        // Si el valor es muy pequeño lo forzamos a cero
        if (_smoothMoveInput.magnitude < 0.01f)
            _smoothMoveInput = Vector2.zero;

        animatorManager?.HandleAnimatorValues(0f, _smoothMoveInput.magnitude);
    }
    private void MoverPersonaje()
    {
        // Rotación tipo tanque (eje X)
        float turn = _moveInput.x * rotationSpeed * Time.fixedDeltaTime;
        Quaternion turnRotation = Quaternion.Euler(0f, turn, 0f);
        _rb.MoveRotation(_rb.rotation * turnRotation);

        // Movimiento adelante/atrás (eje Y)
        Vector3 moveDirection = transform.forward * _moveInput.y * moveSpeed;
        _rb.linearVelocity = new Vector3(moveDirection.x, _rb.linearVelocity.y, moveDirection.z);
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Colisionado con enemigo: " + collision.gameObject.name);
            TakeDamage(1); // Ejemplo: el jugador recibe 1 punto de daño al colisionar con un enemigo
        }
    }
    
    public void RegenerateHealth()
    {
        health += healthRegen;
        health = Mathf.Clamp(health, 0, maxHealth);
        // Aquí podrías agregar efectos de regeneración, sonidos, etc.
    }
    public void TakeDamage(int damage)
    {
        health -= damage;
        health = Mathf.Clamp(health, 0, maxHealth);
        if (health <= 0)
        {
            // Aquí podrías manejar la muerte del jugador, como reproducir una animación, reiniciar el nivel, etc.
            Debug.Log("Jugador ha muerto");
            Destroy(gameObject); // Ejemplo: destruir el objeto del jugador al morir
        }
        Debug.Log("Salud restante: " + health);
        // Aquí podrías agregar efectos de daño, sonidos, etc.
    }
}