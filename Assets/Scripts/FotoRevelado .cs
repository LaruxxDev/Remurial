using UnityEngine;
using UnityEngine.InputSystem;

public class FotoRevelado  : MonoBehaviour
{

    [SerializeField] private InputActionReference revealAction; 

    [Header("Referencia a los datos")]
    public DatosFotos datos; // asignado al instanciar la foto

    [Header("Configuración")]
    public float shakeBoost = 3f; // multiplicador al agitar con R1+L1
    
    [Tooltip("Arrastra aquí tu cubo o GameObject desde el Inspector")]
    public GameObject reveal; // referencia al objeto que se volverá invisible 

    private Material materialInstanciado; // Aquí guardaremos el material extraído
    private bool reveladoCompleto = false;

    void Start()
    {
        // Extraemos el material del reveal justo al empezar
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
        if (datos == null || reveladoCompleto) return;
        if (datos.revealTime <= 0f)
        {
            datos.revealProgress = 1f;
        }
        // 1. Avance automático con el tiempo
        float velocidad = 1f / datos.revealTime;
        datos.revealProgress += Time.deltaTime * velocidad;
        Debug.Log($"Foto opacidad {datos.revealProgress}.");
        // 2. Bonus al agitar con R1 + L1
        bool agitando = revealAction.action.IsPressed(); 
        if (agitando)
        {
            datos.revealProgress += Time.deltaTime * shakeBoost;
        }

        // 3. Limitar el progreso para que no pase de 1 ni baje de 0
        datos.revealProgress = Mathf.Clamp01(datos.revealProgress);

        // 4. Actualizar el material (usando el que extrajimos en el Start)
        if (materialInstanciado != null)
        {
            // De opaco a invisible según el progreso (1 → 0)
            if (materialInstanciado.HasProperty("_Color"))
            {
                Color colorActual = materialInstanciado.color;
                colorActual.a = 1f - datos.revealProgress; 
                materialInstanciado.color = colorActual;
            }
        }

        // 5. Comprobar si acaba de completarse
        if (datos.revealProgress >= 1f)
        {
            reveladoCompleto = true;
            OnReveladoCompleto();
        }
    }

    private void OnReveladoCompleto()
    {
        // Evento opcional: sonido, partículas, destruir el objeto, etc.
        Debug.Log($"Foto {datos.idFoto} ha desaparecido completamente.");
    }
}