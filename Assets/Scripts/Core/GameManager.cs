using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Esto nos permite llamar a este script desde cualquier otro archivo
    public static GameManager Instancia; 

    [Header("Configuración")]
    [SerializeField] private Image imagenNegra;
    [SerializeField] private float velocidadFade = 1.5f; // A mayor número, más rápido se hace el fade
    [SerializeField] private int frameRate = 30;
    public Transform spawnPoint;

    private bool estaPausado = false;

    private void Awake()
    {
        // Configuramos el Singleton
        if (Instancia == null) 
        {
            Instancia = this;
        }
        else 
        {
            Destroy(gameObject);
        }

        ApplyOptions();
    }

    private void Start()
    {
        StartCoroutine(DelayedStart()); // SaveSystem
    }

    #region Puzles
    public void ResolverPuzle(int idPuzle)
    {
        Debug.Log($"Puzle resuelto: {idPuzle}");
        // Aquí puedes agregar lógica adicional para manejar el puzle resuelto, como abrir una puerta o activar un evento.
    }

    #endregion

    #region Funciones de Pausa y Blackout
    public void PausarJuego()
    {
        Time.timeScale = 0f; 
        estaPausado = true;
    }

    public void ReanudarJuego()
    {
        Time.timeScale = 1f;
        estaPausado = false;
    }

    public void TogglePausa()
    {
        if (estaPausado) ReanudarJuego();
        else PausarJuego();
    }

    #region Fade
    // ── Llama a esta función para que la pantalla se ponga negra ──
    public void HacerBlackout()
    {
        StopAllCoroutines(); // Detenemos cualquier fade anterior
        StartCoroutine(RutinaFade(1f)); // 1f significa 100% opaco
    }

    // ── Llama a esta función para volver a ver el juego ──
    public void QuitarBlackout()
    {
        StopAllCoroutines();
        StartCoroutine(RutinaFade(0f)); // 0f significa totalmente transparente
    }

    private IEnumerator RutinaFade(float alphaObjetivo)
    {
        // Guardamos el color y la transparencia actual
        Color colorActual = imagenNegra.color;
        float alphaInicial = colorActual.a;

        float progreso = 0f;

        // Bucle que cambia gradualmente el Alpha de la imagen
        while (progreso < 1f)
        {
            progreso += Time.deltaTime * velocidadFade;
            
            // Mathf.Lerp hace una transición suave entre el valor inicial y el final
            float nuevoAlpha = Mathf.Lerp(alphaInicial, alphaObjetivo, progreso);
            
            imagenNegra.color = new Color(colorActual.r, colorActual.g, colorActual.b, nuevoAlpha);
            
            yield return null; // Esperamos al siguiente frame
        }
    }
    #endregion
    #endregion

    #region Opciones
    private void ApplyOptions()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = frameRate;
    }
    #endregion

    #region Save&Load    
    [Header("Save Locations")]
    [SerializeField] private PlayerConfiguration player;    // Referencia al player
    public PlayerConfiguration PLAYER
    {
        get => player;
        set => player = value;
    }

    private IEnumerator DelayedStart()
    {
        yield return new WaitUntil(() => player != null);
        yield return new WaitForFixedUpdate();

        SaveSystem.Initialize();
    }
    #endregion
}