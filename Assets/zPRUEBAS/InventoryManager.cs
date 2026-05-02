using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class InventoryManager : MonoBehaviour
{
    #region Values
    public static InventoryManager Instance { get; private set; }


    [Header("Configuración")]
    [SerializeField] private int maxItems = 20;

    public List<Item> itemsList = new List<Item>();
    private int _index = 0;

    private bool _isInventoryOpen = false;


    [SerializeField] private PlayerGeneral PLAYER;



    [Header("UI")]
    [SerializeField] private VisualElement _root;
    [SerializeField] private VisualElement _mainContainer;
    [SerializeField] private VisualElement _carrusel;
    [SerializeField] private VisualElement _bigItemImage;
    [SerializeField] private Label _labelName;
    [SerializeField] private Label _labelDesc;
    [SerializeField] private Button _actionButton;


    // Eventos
    public event Action<Item> OnItemAgregado;
    public event Action<Item> OnItemEliminado;
    public event Action OnInventarioLleno;
    #endregion


    #region Unity Methods
    void Awake()
    {
        // Singleton
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

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

    void OnDestroy()
    {
        LiberarTodasLasTexturas();
    }
    #endregion

    #region Inventory Methods
    public bool AgregarItem(Item item)
    {
        if (item == null)
        {
            Debug.LogError("No se puede agregar un item nulo.");
            return false;
        }

        if (itemsList.Count >= maxItems)
        {
            Debug.LogWarning("Inventario lleno.");
            OnInventarioLleno?.Invoke();
            return false;
        }

        itemsList.Add(item);
        OnItemAgregado?.Invoke(item);

        if (_isInventoryOpen) UpdateUI();

        Debug.Log($"Item agregado: {item.name} | Total: {itemsList.Count}/{maxItems}");
        return true;
    }

    public bool EliminarItem(int id)
    {
        Item item = BuscarItem(id);
        if (item == null)
        {
            Debug.LogWarning("No se encontró el item con id: " + id);
            return false;
        }

        // Si es foto, liberar su textura antes de eliminar
        if (item.esFoto && item.datosFoto != null)
        {
            item.datosFoto.LiberarTextura();
            Debug.Log($"Textura liberada al eliminar item: {item.name}");
        }

        itemsList.Remove(item);
        OnItemEliminado?.Invoke(item);

        // Ajustar índice si quedó fuera de rango
        if (_index >= itemsList.Count)
        {
            _index = Mathf.Max(0, itemsList.Count - 1);
        }

        if (_isInventoryOpen) UpdateUI();

        Debug.Log($"Item eliminado: {item.name} | Total: {itemsList.Count}/{maxItems}");
        return true;
    }

    public Item BuscarItem(int id)
    {
        return itemsList.Find(i => i.id == id);
    }

    public bool InventarioLleno() => itemsList.Count >= maxItems;
    public int EspacioRestante() => maxItems - itemsList.Count;

    private void LiberarTodasLasTexturas()
    {
        foreach (Item item in itemsList)
        {
            if (item.esFoto && item.datosFoto != null)
            {
                item.datosFoto.LiberarTextura();
                Debug.Log($"Textura liberada al cerrar inventario: {item.name}");
            }
        }
    }
    #endregion

    #region UI Methods
    private void ConfigurarEventos()
    {

        // BORRABLE?
        //_root.RegisterCallback<NavigationMoveEvent>(evt =>
        //{
        //    if (!_isInventoryOpen || itemsList.Count == 0) return;

        //    if (evt.direction == NavigationMoveEvent.Direction.Right)
        //    {
        //        _index = (_index + 1) % itemsList.Count;
        //        UpdateUI();
        //    }
        //    else if (evt.direction == NavigationMoveEvent.Direction.Left)
        //    {
        //        _index = (_index - 1 + itemsList.Count) % itemsList.Count;
        //        UpdateUI();
        //    }
        //});

        // Botón Derecho
        _root.Q<Button>("RightButton").clicked += () =>
        {
            if (itemsList.Count == 0) return;
            _index = (_index + 1) % itemsList.Count;
            UpdateUI();
        };

        // Botón Izquierdo
        _root.Q<Button>("LeftButton").clicked += () =>
        {
            if (itemsList.Count == 0) return;
            _index = (_index - 1 + itemsList.Count) % itemsList.Count;
            UpdateUI();
        };

        // Botón Inspeccionar
        _actionButton.clicked += () =>
        {
            if (itemsList.Count == 0)
                return;

            TryActionCurrentItem(out bool wantsInspect, out GameObject inspectTarget);

            if (wantsInspect && inspectTarget != null)
            {
                PLAYER.inspectionItem = inspectTarget;
                PLAYER.STATEMACHINE.ChangeState(PLAYER.STATES.InspectState(PLAYER.STATEMACHINE));
            }
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

        _actionButton.style.display = DisplayStyle.Flex;
        _carrusel.Clear();

        var currentItem = itemsList[_index];
        int prevIndex = (_index - 1 + itemsList.Count) % itemsList.Count;
        int nextIndex = (_index + 1) % itemsList.Count;

        if (itemsList.Count > 1)
            _carrusel.Add(CrearItemCarrusel(itemsList[prevIndex], false));

        _carrusel.Add(CrearItemCarrusel(itemsList[_index], true));

        if (itemsList.Count > 2)
            _carrusel.Add(CrearItemCarrusel(itemsList[nextIndex], false));

        // Si el item actual es foto, cargar su textura para mostrarla
        if (currentItem.esFoto && currentItem.datosFoto != null)
        {
            currentItem.datosFoto.CargarTextura();
            if (currentItem.datosFoto.textura != null)
            {
                Sprite spriteTextura = TexturaASprite(currentItem.datosFoto.textura);
                _bigItemImage.style.backgroundImage = new StyleBackground(spriteTextura);
            }
        }
        else
        {
            _bigItemImage.style.backgroundImage = new StyleBackground(currentItem.sprite);
        }

        _bigItemImage.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
        _labelName.text = currentItem.name.ToUpper();
        _labelDesc.text = currentItem.description;
        _actionButton.text = currentItem.isUsable ? "USE" : "EXAMINE";
    }

    // Convierte Texture2D a Sprite para mostrarlo en UI Toolkit
    private Sprite TexturaASprite(Texture2D textura)
    {
        return Sprite.Create(textura, new Rect(0, 0, textura.width, textura.height), new Vector2(0.5f, 0.5f));
    }


    private VisualElement CrearItemCarrusel(Item item, bool esSeleccionado)
    {
        VisualElement ve = new VisualElement();
        ve.AddToClassList("carrusel-item");
        ve.AddToClassList(esSeleccionado ? "carrusel-item-selected" : "carrusel-item-unselected");
        ve.style.width = 120;
        ve.style.height = 120;
        ve.style.flexShrink = 0;

        // Si es foto usamos su textura, si no su sprite normal
        if (item.esFoto && item.datosFoto != null)
        {
            item.datosFoto.CargarTextura();
            if (item.datosFoto.textura != null)
            {
                ve.style.backgroundImage = new StyleBackground(TexturaASprite(item.datosFoto.textura));
                ve.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
                ve.style.unityBackgroundImageTintColor = Color.white;
            }
            else
            {
                ve.style.backgroundColor = Color.magenta;
            }
        }
        else if (item.sprite != null)
        {
            ve.style.backgroundImage = new StyleBackground(item.sprite);
            ve.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
            ve.style.unityBackgroundImageTintColor = Color.white;
        }
        else
        {
            ve.style.backgroundColor = Color.magenta;
        }

        return ve;
    }
    #endregion



    public void OpenInventory()
    {
        _isInventoryOpen = true;
        _mainContainer.style.display = DisplayStyle.Flex;
        UpdateUI();
        _root.Focus();
    }

    public void CloseInventory()
    {
        _isInventoryOpen = false;
        _mainContainer.style.display = DisplayStyle.None;
    }

    private float _navCooldown = 0f;
    private const float NAV_COOLDOWN_TIME = 0.02f;

    public void NavigateRight()
    {
        if (itemsList.Count == 0 || Time.unscaledTime < _navCooldown)
            return;

        _index = (_index + 1) % itemsList.Count;
        _navCooldown = Time.unscaledTime + NAV_COOLDOWN_TIME;
        UpdateUI();
    }

    public void NavigateLeft()
    {
        if (itemsList.Count == 0 || Time.unscaledTime < _navCooldown) 
            return;

        _index = (_index - 1 + itemsList.Count) % itemsList.Count;
        _navCooldown = Time.unscaledTime + NAV_COOLDOWN_TIME;
        UpdateUI();
    }

    public void TryActionCurrentItem(out bool wantsInspect, out GameObject inspectTarget)
    {
        wantsInspect = false;
        inspectTarget = null;

        if (itemsList.Count == 0)
            return;

        var item = itemsList[_index];

        if (item.isUsable)
        {
            // Usar
        }
        else
        {
            inspectTarget = item.ObtenerGameObjectParaInspeccion();

            if (inspectTarget != null)
            {
                wantsInspect = true;
                EliminarItem(item.id);
            }
        }
    }
}