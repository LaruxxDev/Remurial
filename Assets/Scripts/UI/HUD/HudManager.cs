using UnityEngine;
using UnityEngine.UIElements;

public class HudManager : MonoBehaviour
{
    public static HudManager Instance { get; private set; }

    private VisualElement _root;
    private VisualElement _hudContainer;
    private VisualElement _notificacionContainer;
    private Label _textoNotificacion;

    // Tiempo que el texto permanece visible (en segundos)
    [SerializeField] private float tiempoVisible = 5.0f; 


    void Start() {
        MostrarMensaje("SISTEMA DE HUD ACTIVO");
    }
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

        _root = GetComponent<UIDocument>().rootVisualElement;
        _hudContainer = _root.Q<VisualElement>("HudRoot");
        _notificacionContainer = _root.Q<VisualElement>("NotificacionContainer");
        _textoNotificacion = _root.Q<Label>("TextNotificacion");

        OcultarNotificacion();
    }

    public void MostrarMensaje(string mensaje)
    {
        CancelInvoke("IniciarDesvanecimiento");

        _textoNotificacion.text = mensaje;

        _notificacionContainer.RemoveFromClassList("notificacion-oculta");
        _notificacionContainer.style.display = DisplayStyle.Flex;
        _notificacionContainer.style.display = DisplayStyle.Flex;
        _notificacionContainer.style.opacity = 1f;
        _notificacionContainer.style.backgroundColor = Color.red;

        Invoke("IniciarDesvanecimiento", tiempoVisible);
    }

    private void IniciarDesvanecimiento()
    {
        _notificacionContainer.AddToClassList("notificacion-oculta");
        
        // Espera a que termine la animación
        Invoke("FinalizarOcultacion", 0.5f);
    }

    private void FinalizarOcultacion()
    {
        
        if (_notificacionContainer.ClassListContains("notificacion-oculta"))
        {
            _notificacionContainer.style.display = DisplayStyle.None;
        }
    }

    private void OcultarNotificacion()
    {
        _notificacionContainer.AddToClassList("notificacion-oculta");
        _notificacionContainer.style.display = DisplayStyle.None;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.T)) {
            MostrarMensaje("¡PRUEBA DE TEXTO EXITOSA!");
        }
    }
}