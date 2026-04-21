using UnityEngine;

public class BestiarioManager : MonoBehaviour
{
//    public List<EnemyDatabase> todasLasEntradas;
//    private VisualElement _root;
//    private Label _labelTitulo, _labelCuerpo;
//    private VisualElement _fotoContenedor;
//
//    void Awake()
//    {
//        _root = GetComponent<UIDocument>().rootVisualElement;
//
////     // Referencias a las etiquetas en tus páginas
////     _labelTitulo = _root.Q<Label>("NombreEntidad"); // Pon este nombre en UI Builder
////     _labelCuerpo = _root.Q<Label>("TextoDescripcion");
////     _fotoContenedor = _root.Q<VisualElement>("FotoEntidad");
////
//        // Configurar botones de pestañas
//        _root.Q<Button>("HospitalButton").clicked += () => MostrarCategoria("Hospital");
//        _root.Q<Button>("IglesiaButton").clicked += () => MostrarCategoria("Iglesia");
//    }
//
//    void MostrarCategoria(string categoria)
//    {
//        // Buscamos la primera entrada que coincida con la categoría
//        var entrada = todasLasEntradas.FirstOrDefault(e => e.categoria == categoria);
//        
//        if (entrada != null)
//        {
//            _labelTitulo.text = entrada.nombreEntidad.ToUpper();
//            _labelCuerpo.text = entrada.descripcion;
//            _fotoContenedor.style.backgroundImage = new StyleBackground(entrada.fotoEntidad);
//        }
//    }
}
