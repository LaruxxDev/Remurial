using UnityEngine;
using UnityEngine.InputSystem;

public class RevealPhoto : MonoBehaviour
{
    [Header("Player")]
    public PlayerGeneral PLAYER;

    [Header("Referencia a los datos")]
    public DatosFotos datos;                    // Asignado al instanciar la foto

    [Header("Configuración del Revelado")]
    public float shakeReduction;                // Reducción por input
    public float remainingTime;                 // Tiempo Restante

    [Header("Configuración Visual del Agite")]
    public float distanciaAgite = 0.15f;        // Cuánto se mueve hacia arriba/abajo
    public float velocidadRetorno = 12f;        // Qué tan rápido vuelve al centro la foto

    [Tooltip("Arrastra aquí tu cubo o GameObject desde el Inspector")]
    public GameObject reveal; 

    private Material materialInstanciado;
    public bool reveladoCompleto = false;

    // Variables para controlar el movimiento visual
    private Vector3 posicionOriginal;
    private Vector3 targetOffset;
    private Vector3 currentOffset;
    public bool isInspecting = false;
    private bool shakeUp = true;



    void Start()
    {
        // Referencia del Player
        PLAYER = FindAnyObjectByType<PlayerGeneral>();
        PLAYER.heldPhoto = this;

        // Valores iniciales
        posicionOriginal = transform.localPosition;
        remainingTime = datos.revealTime;

        if (reveal != null)
        {
            Renderer rendererCubo = reveal.GetComponent<Renderer>();
            
            if (rendererCubo != null)            
                materialInstanciado = rendererCubo.material;           
            else           
                Debug.LogWarning($"El objeto {reveal.name} no tiene un componente Renderer.");            
        }       
    }

    void Update()
    {
        if (reveladoCompleto)
            OnReveladoCompleto();

        if (datos == null || reveladoCompleto || isInspecting) 
            return;

        remainingTime -= Time.deltaTime;

        datos.revealProgress = 1f - Mathf.Clamp01(remainingTime / datos.revealTime);

        currentOffset = Vector3.Lerp(currentOffset, targetOffset, Time.deltaTime * velocidadRetorno);
        targetOffset = Vector3.Lerp(targetOffset, Vector3.zero, Time.deltaTime * velocidadRetorno * 0.5f);
        transform.localPosition = posicionOriginal + currentOffset;


        if (materialInstanciado != null && materialInstanciado.HasProperty("_Color"))
        {
            Color colorActual = materialInstanciado.color;
            colorActual.a = 1f - datos.revealProgress;
            materialInstanciado.color = colorActual;
        }

        if (remainingTime <= 0f)
        {
            remainingTime = 0f;
            datos.revealProgress = 1f;
            reveladoCompleto = true;
            transform.localPosition = posicionOriginal;
            OnReveladoCompleto();
        }
    }

    public void ShakeBoost()
    {
        remainingTime -= shakeReduction;
        remainingTime = Mathf.Max(remainingTime, 0f);

        targetOffset = shakeUp
            ? new Vector3(0f, distanciaAgite, 0f)
            : new Vector3(0f, -distanciaAgite, 0f);

        shakeUp = !shakeUp;
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
        if (reveladoCompleto == false)
            return;

        reveladoCompleto = false;

        if (datos.enemiesCaught == null)
            return;

        foreach (EnemyCollision enemy in datos.enemiesCaught)
        {
            enemy.REVEALED = true;
        }

        // Destroy
        Destroy(gameObject);
    }
}