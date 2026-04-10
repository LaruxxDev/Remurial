using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class InventarioManager : MonoBehaviour
{
    
    public List<Item> itemsList;
    private int _index = 0;

   // Elementos de la UI
    private VisualElement _root;
    private VisualElement _mainContainer; // El contenedor principal de tu UI Builder
    private VisualElement _carrusel;
    private VisualElement _bigItemImage;
    private Label _labelName;
    private Label _labelDesc;
    private Button _actionButton;

    private bool _isInventoryOpen = false;

    private InspectSystem _inspectSystem;

    void Awake()
    {
        
        _root = GetComponent<UIDocument>().rootVisualElement;
        
        _mainContainer = _root.Q<VisualElement>("MainContainer");
        _mainContainer.style.display = DisplayStyle.None;

        _carrusel = _root.Q<VisualElement>("Carrusel");
        _labelName = _root.Q<Label>("ItemName");
        _bigItemImage = _root.Q<VisualElement>("ItemImage");
        _labelDesc = _root.Q<Label>("ItemDesc");
        _actionButton = _root.Q<Button>("ActionButton");

        ConfigurarEventos();
    }

    void Update()
    {
        // Abrir/Cerrar con la tecla "I" o "Tab"
        if (Input.GetKeyDown(KeyCode.I) || Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    private void ToggleInventory()
    {
        _isInventoryOpen = !_isInventoryOpen;

        if (_isInventoryOpen)
        {
            _mainContainer.style.display = DisplayStyle.Flex;
            UpdateUI();
            _root.Focus(); // Para que el mando/teclado funcione al abrir
            
            // Opcional: Pausar el juego
            // Time.timeScale = 0;
        }
        else
        {
            _mainContainer.style.display = DisplayStyle.None;
            // Opcional: Reanudar el juego
            // Time.timeScale = 1;
        }
    }

    private void ConfigurarEventos()
    {
        // Navegació teclad y mando
        _root.RegisterCallback<NavigationMoveEvent>(evt =>
        {
            if (!_isInventoryOpen) return; // Solo si está abierto

            if (evt.direction == NavigationMoveEvent.Direction.Right)
            {
                _index = (_index + 1) % itemsList.Count;
                UpdateUI();
            }
            else if (evt.direction == NavigationMoveEvent.Direction.Left)
            {
                _index = (_index - 1 + itemsList.Count) % itemsList.Count;
                UpdateUI();
            }
        });

        // Navegación con botones
        _root.Q<Button>("RightButton").clicked += () => { _index = (_index + 1) % itemsList.Count; UpdateUI(); };
        _root.Q<Button>("LeftButton").clicked += () => { _index = (_index - 1 + itemsList.Count) % itemsList.Count; UpdateUI(); };

        _actionButton.clicked += () =>
        {
            var item = itemsList[_index];
            if (item.isUsable)
            {
                // Aquí iría la lógica para usar el ítem
                Debug.Log($"Usando {item.name}");
            }
            else
            {
                // Llama al sistema de inspección para mostrar el ítem
                Debug.Log($"Examinando {item.name}");
            }
            Debug.Log(item.isUsable ? $"Usando {item.name}" : $"{item.name} no es usable");
        };

    }

    private void UpdateUI()
    {
        if (itemsList.Count == 0)
        {
            _carrusel.Clear();
            _bigItemImage.style.backgroundImage = null;
            _labelName.text = "EMPTY";
            _labelDesc.text = "No items in inventory.";
            _actionButton.style.display = DisplayStyle.None;
            return;
        }

        if (_carrusel == null)
        {
            Debug.LogError("¡OJO! No se encuentra el VisualElement llamado 'Carrusel'. Revisa el nombre en el UI Builder.");
            return;
        }   

        _actionButton.style.display = DisplayStyle.Flex;
        _carrusel.Clear();
        var currentItem = itemsList[_index];

        // --- LÓGICA DE RUEDA (3 ÍTEMS) ---
    
        // 1. Calcular índices
        int prevIndex = (_index - 1 + itemsList.Count) % itemsList.Count;
        int nextIndex = (_index + 1) % itemsList.Count;

        // 2. Añadir ítem ANTERIOR (Izquierda)
        if (itemsList.Count > 1)
            _carrusel.Add(CrearItemCarrusel(itemsList[prevIndex].sprite, false));

        // 3. Añadir ítem ACTUAL (Centro)
        _carrusel.Add(CrearItemCarrusel(itemsList[_index].sprite, true));

        // 4. Añadir ítem SIGUIENTE (Derecha)
        if (itemsList.Count > 2)
            _carrusel.Add(CrearItemCarrusel(itemsList[nextIndex].sprite, false));

        // --- PANEL LATERAL (Info del ítem actual) ---
        _bigItemImage.style.backgroundImage = new StyleBackground(currentItem.sprite);
        _bigItemImage.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
        _labelName.text = currentItem.name.ToUpper();
        _labelDesc.text = currentItem.description;
        _actionButton.text = currentItem.isUsable ? "USE" : "EXAMINE";
    }

    // Función para generar cada cuadrito de la rueda
    private VisualElement CrearItemCarrusel(Sprite sprite, bool esSeleccionado)
    {
        VisualElement item = new VisualElement();
        
        // Asignamos las clases del USS
        item.AddToClassList("carrusel-item");
        item.AddToClassList(esSeleccionado ? "carrusel-item-selected" : "carrusel-item-unselected");

        // Forzamos dimensiones mínimas por código
        item.style.width = 120;
        item.style.height = 120;
        item.style.flexShrink = 0; // Crucial para que no se aplaste a 0px

        if (sprite != null)
        {
            item.style.backgroundImage = new StyleBackground(sprite);
            item.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
            item.style.unityBackgroundImageTintColor = Color.white; // Asegura que no sea negro  
        }
        else
        {
            // SI VES ESTE COLOR MAGENTA, el problema es que el script no recibe el Sprite
            item.style.backgroundColor = Color.magenta; 
        }

        return item;
    }
}

