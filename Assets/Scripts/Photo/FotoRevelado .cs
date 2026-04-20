using UnityEngine;

public class FotoRevelado : MonoBehaviour
{
    [Header("Input")]
    [SerializeField] private GameInputReader input; 

    [Header("Referencia a los datos")]
    public DatosFotos datos;

    [Header("Configuración del Revelado")]
    public float shakeBoostInicial   = 1f;
    public float incrementoVelocidad = 0.5f;
    public float boostDecaimiento    = 0.8f; 
    private float _currentShakeBoost;

    [Header("Configuración Visual del Agite")]
    public float distanciaAgite   = 0.15f;
    public float velocidadRetorno = 12f;

    public GameObject reveal;

    private Material  _material;
    private bool      _reveladoCompleto = false;
    private Vector3   _posicionOriginal;
    private Vector3   _targetOffset;
    private Vector3   _currentOffset;
    public  bool      isInspecting = false;

    // ── Suscripción ──────────────────────────────────────────────────────
    private void OnEnable()
    {
        // Solo nos suscribimos si el input está asignado
        if (input == null) return;
        input.OnRevealUp   += HandleRevealUp;
        input.OnRevealDown += HandleRevealDown;
    }

    private void OnDisable()
    {
        if (input == null) return;
        input.OnRevealUp   -= HandleRevealUp;
        input.OnRevealDown -= HandleRevealDown;
    }

    // ── Handlers de input ──────────────────────────────────────────────────────
    private void HandleRevealUp()
    {
        if (_reveladoCompleto || isInspecting || datos == null) return;
        _targetOffset      = new Vector3(0, distanciaAgite, 0);
        _currentShakeBoost += incrementoVelocidad;
        // Debug para confirmar que el input llega
        Debug.Log($"[FotoRevelado] RevealUp — boost: {_currentShakeBoost:F2}");
    }

    private void HandleRevealDown()
    {
        if (_reveladoCompleto || isInspecting || datos == null) return;
        _targetOffset      = new Vector3(0, -distanciaAgite, 0);
        _currentShakeBoost += incrementoVelocidad;
        Debug.Log($"[FotoRevelado] RevealDown — boost: {_currentShakeBoost:F2}");
    }

    // ── Ciclo de vida ────────────────────────────────────────────────────
    private void Start()
    {
        _currentShakeBoost = shakeBoostInicial;
        _posicionOriginal  = transform.localPosition;

        if (reveal == null) return;
        var r = reveal.GetComponent<Renderer>();
        if (r == null) return;

        _material = r.material;
        if (_material.HasProperty("_Color"))
        {
            Color c = _material.color;
            c.a = datos.revealProgress;
            _material.color = c;
        }
    }

    private void Update()
    {
        if (datos == null || _reveladoCompleto || isInspecting) return;

        // ── 1. Avance automático + boost del agite ───────────────────────
        float velocidadBase  = 1f / Mathf.Max(datos.revealTime, 0.01f);
        // currentShakeBoost ahora SÍ multiplica la velocidad
        float velocidadTotal = velocidadBase * _currentShakeBoost;
        datos.revealProgress += Time.deltaTime * velocidadTotal;

        // El boost decae solo para que agitar seguido tenga más impacto
        _currentShakeBoost = Mathf.Max(
            shakeBoostInicial,
            Mathf.Lerp(_currentShakeBoost, shakeBoostInicial, Time.deltaTime * boostDecaimiento)
        );

        // ── 2. Movimiento visual ─────────────────────────────────────────
        _currentOffset = Vector3.Lerp(_currentOffset, _targetOffset, Time.deltaTime * velocidadRetorno);
        _targetOffset  = Vector3.Lerp(_targetOffset, Vector3.zero, Time.deltaTime * velocidadRetorno * 0.5f);
        transform.localPosition = _posicionOriginal + _currentOffset;

        // ── 3. Aplicar al material ───────────────────────────────────────
        datos.revealProgress = Mathf.Clamp01(datos.revealProgress);
        if (_material != null && _material.HasProperty("_Color"))
        {
            Color c = _material.color;
            c.a = 1f - datos.revealProgress;
            _material.color = c;
        }

        // ── 4. Comprobar completado ──────────────────────────────────────
        if (datos.revealProgress >= 1f)
        {
            _reveladoCompleto       = true;
            transform.localPosition = _posicionOriginal;
            OnReveladoCompleto();
        }
    }

    // ── API pública ──────────────────────────────────────────────────────
    public void RevelarInstantaneo()
    {
        _reveladoCompleto = true;
        isInspecting      = true;
        if (datos != null) datos.revealProgress = 1f;

        if (_material != null && _material.HasProperty("_Color"))
        {
            Color c = _material.color;
            c.a = 0f;
            _material.color = c;
        }
        if (reveal != null) reveal.SetActive(false);
    }

    private void OnReveladoCompleto()
    {
        Debug.Log($"[FotoRevelado] {datos.idFoto} revelada completamente.");
        // GameEvents.OnFotoRevelada?.Invoke(datos);
        if (datos.enemigosCapturados.Count > 0)
        {
            foreach (var enemigo in datos.enemigosCapturados)
            {
                var collision = enemigo.GetComponentInChildren<EnemyCollision>();
                if (collision != null)
                {
                    collision.REVEALED = true;
                    Destroy(enemigo);
                    Debug.Log($"Enemigo revelado: {enemigo.name}");
                }
                else
                {
                    Debug.LogWarning($"{enemigo.name} no tiene EnemyCollision.");
                }
            }
        }
    }
}