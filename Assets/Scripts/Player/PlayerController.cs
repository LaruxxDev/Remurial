using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    public enum PlayerState { Idle, Walking, TakingPhoto, Revealing, Interacting }
    private PlayerState _currentState = PlayerState.Idle;

    [Header("Input")]
    [SerializeField] private GameInputReader _input;

    [Header("Movimiento")]
    public float moveSpeed = 5f;
    public float aimSpeedMultiplier = 0.5f;
    public float rotationSpeed = 120f;

    [Header("Vida")]
    public int health = 6;
    public  int maxHealth = 6;
    public int healthRegen = 1;

    [Header("Referencias")]
    public AnimatorManager animatorManager;
    [SerializeField] private Animator _animator;
    [SerializeField] private Transform _cameraTransform; // arrastra el transform de tu Cinemachine Brain o Main Camera

    private Rigidbody _rb;
    private Vector2 _smoothMoveInput;
    private bool _estaApuntando = false; // ← estado del aim

    private Vector2 _moveInput;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void OnEnable()
    {
        _input.OnMoveEvent         += HandleMove;
        _input.OnAimStarted     += HandleAimStart;
        _input.OnAimCanceled    += HandleAimEnd;
        _input.OnRevealUp          += HandleReveal;
        _input.OnInteractPerformed += HandleInteract;
    }

    private void OnDisable()
    {
        _input.OnMoveEvent         -= HandleMove;
        _input.OnAimStarted     -= HandleAimStart;
        _input.OnAimCanceled    -= HandleAimEnd;
        _input.OnRevealUp          -= HandleReveal;
        _input.OnInteractPerformed -= HandleInteract;
    }

    private void FixedUpdate()
    {
        if (_currentState == PlayerState.Interacting) return;
        if (_estaApuntando)
        {
            MoverPersonajeFPS();
        }
        else
        {
            MoverPersonaje();
        }
    }

    // ── Input Handlers ───────────────────────────────────────

    private void HandleMove(Vector2 dir)
    {
        if (_currentState == PlayerState.Interacting) return;

        _moveInput = dir;
        _currentState = dir != Vector2.zero ? PlayerState.Walking : PlayerState.Idle;

    }

// ── Handlers ─────────────────────────────────────────────────────────

    private void HandleAimStart()
    {
        if (_currentState == PlayerState.Interacting) return;
        _estaApuntando = true;
        _currentState  = PlayerState.TakingPhoto;
        //_animator.SetTrigger("TakePhoto");
    }

    private void HandleAimEnd()
    {
        _estaApuntando = false;
        // Solo vuelve a Idle/Walking si no está haciendo otra cosa
        if (_currentState == PlayerState.TakingPhoto)
            _currentState = _moveInput != Vector2.zero ? PlayerState.Walking : PlayerState.Idle;
    }

    private void HandleReveal()
    {
        if (_currentState == PlayerState.Interacting) return;

        _currentState = PlayerState.Revealing;
        //_animator.SetTrigger("Reveal");
    }

    private void HandleInteract()
    {
        _currentState = PlayerState.Interacting;
        _moveInput = Vector2.zero;

        if (animatorManager != null)
            animatorManager.HandleAnimatorValues(0f, 0f);

        //_animator.SetTrigger("Interact");
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
    private void MoverPersonajeFPS()
    {
        if (_cameraTransform == null) return;

        // ── 1. Movimiento relativo a la cámara (sin cambiar) ──
        Vector3 forward = _cameraTransform.forward;
        Vector3 right   = _cameraTransform.right;
        forward.y = 0f;
        right.y   = 0f;
        forward.Normalize();
        right.Normalize();

        Vector3 moveDirection = (forward * _moveInput.y + right * _moveInput.x).normalized;
        _rb.linearVelocity = new Vector3(
            moveDirection.x * (moveSpeed * aimSpeedMultiplier),
            _rb.linearVelocity.y,
            moveDirection.z * (moveSpeed * aimSpeedMultiplier)
        );

        // ── 2. El cuerpo rota hacia donde mira la cámara (solo eje Y) ──
        // Usamos el forward de la cámara aplanado, sin importar si hay input.
        Vector3 camForwardFlat = _cameraTransform.forward;
        camForwardFlat.y = 0f;

        if (camForwardFlat.sqrMagnitude > 0.001f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(camForwardFlat);

            // Slerp suave: el cuerpo "sigue" a la cámara con un ligero retraso
            // Cambia 15f por un valor más bajo (5f) si quieres más retraso,
            // o más alto (30f) si quieres que sea casi instantáneo.
            _rb.MoveRotation(Quaternion.Slerp(
                _rb.rotation,
                targetRotation,
                Time.fixedDeltaTime * 15f
            ));
        }
    }

    public void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            TakeDamage(1); 
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
            Die();
        }
        Debug.Log("Salud restante: " + health);
        // Aquí podrías agregar efectos de daño, sonidos, etc.
    }

    public void Die()
    {
        // Aquí podrías manejar la muerte del jugador, como reproducir una animación, reiniciar el nivel, etc.
        Debug.Log("Jugador ha muerto");
        Destroy(gameObject); // Ejemplo: destruir el objeto del jugador al morir
    }
}