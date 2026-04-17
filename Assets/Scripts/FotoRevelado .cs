using UnityEngine;
using UnityEngine.InputSystem;

public class FotoRevelado  : MonoBehaviour
{
    [Header("Controles de Agite")]
    [Tooltip("Asigna aquí la acción del botón R1 (Agitar Arriba)")]
    [SerializeField] private InputActionReference r1Action; 
    [Tooltip("Asigna aquí la acción del botón L1 (Agitar Abajo)")]
    [SerializeField] private InputActionReference l1Action;

    [Header("Referencia a los datos")]
    public PhotoData datos; // asignado al instanciar la foto

    [Header("Configuración del Revelado")]
    public float shakeBoostInicial = 1f; // Multiplicador inicial al agitar
    public float incrementoVelocidad = 0.5f; // Cuánto aumenta la velocidad por CADA pulsación
    private float currentShakeBoost; // El boost actual que irá creciendo

    [Header("Configuración Visual del Agite")]
    public float distanciaAgite = 0.15f; // Cuánto se mueve hacia arriba/abajo
    public float velocidadRetorno = 12f; // Qué tan rápido vuelve al centro la foto

    [Tooltip("Arrastra aquí tu cubo o GameObject desde el Inspector")]
    public GameObject reveal; 

    private Material materialInstanciado;
    private bool reveladoCompleto = false;

    // Variables para controlar el movimiento visual
    private Vector3 posicionOriginal;
    private Vector3 targetOffset;
    private Vector3 currentOffset;
    public bool isInspecting = false;

    void Start()
    {
        // Inicializamos el boost y guardamos la posición donde aparece la foto
        currentShakeBoost = shakeBoostInicial;
        posicionOriginal = transform.localPosition;

        if (reveal != null)
        {
            Renderer rendererCubo = reveal.GetComponent<Renderer>();
            
            if (rendererCubo != null)
            {
                materialInstanciado = rendererCubo.material;
            }
            else
            {
                Debug.LogWarning($"El objeto {reveal.name} no tiene un componente Renderer.");
            }
        }
        
    }

    void Update()
    {
        if (datos == null || reveladoCompleto || isInspecting) return;
        if (datos.revealTime <= 0f)
        {
            datos.revealProgress = 1f;
        }

        // 1. Avance automático con el tiempo
        float velocidadEstandar = 1f / datos.revealTime;
        datos.revealProgress += Time.deltaTime * velocidadEstandar;

        // 2. Detectar cuándo se PULSAN los botones (para dar el "golpe" visual y aumentar velocidad)
        // Usamos WasPressedThisFrame para detectar el click exacto, no si se mantiene calcado.
        bool pulsoR1 = r1Action != null && r1Action.action.WasPressedThisFrame();
        bool pulsoL1 = l1Action != null && l1Action.action.WasPressedThisFrame();

        if (pulsoR1)
        {
            targetOffset = new Vector3(0, distanciaAgite, 0); // Golpecito hacia arriba
            currentShakeBoost += incrementoVelocidad;         // Aceleramos el revelado
        }
        else if (pulsoL1)
        {
            targetOffset = new Vector3(0, -distanciaAgite, 0); // Golpecito hacia abajo
            currentShakeBoost += incrementoVelocidad;          // Aceleramos el revelado
        }


        // --- 4. LÓGICA DE MOVIMIENTO VISUAL (EL AGITE) ---
        // Movemos el offset actual hacia el objetivo suavemente
        currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * velocidadRetorno);
        // Hacemos que el objetivo tienda a volver a cero (el centro) automáticamente
        targetOffset = Vector3.Lerp(targetOffset, Vector3.zero, Time.deltaTime * velocidadRetorno * 0.5f);
        
        // Aplicamos la posición final a la foto
        transform.localPosition = posicionOriginal + currentOffset;
        // --------------------------------------------------

        // 5. Limitar el progreso para que no pase de 1 ni baje de 0
        datos.revealProgress = Mathf.Clamp01(datos.revealProgress);

        // 6. Actualizar el material (usando el que extrajimos en el Start)
        if (materialInstanciado != null && materialInstanciado.HasProperty("_Color"))
        {
            Color colorActual = materialInstanciado.color;
            colorActual.a = 1f - datos.revealProgress; 
            materialInstanciado.color = colorActual;
        }
        

        // 7. Comprobar si acaba de completarse
        if (datos.revealProgress >= 1f)
        {
            reveladoCompleto = true;
            transform.localPosition = posicionOriginal; // Aseguramos que quede centrada al terminar
            OnReveladoCompleto();
        }
    }

    public void RevelarInstantaneo()
    {
        reveladoCompleto = true;
        isInspecting = true; // Esto detendrá el Update por la condición al inicio del mismo
        
        if (datos != null)
        {
            datos.revealProgress = 1f;
        }

        if (materialInstanciado != null && materialInstanciado.HasProperty("_Color"))
        {
            Color c = materialInstanciado.color;
            c.a = 0f; // Hacemos la tapa totalmente transparente
            materialInstanciado.color = c;
        }
        
        // Si tienes el objeto 'reveal' (el cubo), lo desactivamos directamente
        if (reveal != null) reveal.SetActive(false); 
    }
    private void OnReveladoCompleto()
    {
        Debug.Log($"Foto {datos.photoID} ha desaparecido completamente.");
    }
}