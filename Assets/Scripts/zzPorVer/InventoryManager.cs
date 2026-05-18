using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using Unity.VisualScripting;

public class InventoryManager : MonoBehaviour
{
    #region Values
    public static InventoryManager Instance { get; private set; }


    [Header("Configuraci�n")]
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
    private Button _examineButton;
    private Button _cancelButton;


    [Header("Save")]
    [SerializeField] private ItemDatabase itemDatabase;

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
        _examineButton = _root.Q<Button>("ExamineButton");
        _cancelButton = _root.Q<Button>("CancelButton");  

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

        Debug.Log($"Item agregado: {item.itemName} | Total: {itemsList.Count}/{maxItems}");
        return true;
    }

    public bool EliminarItem(int id)
    {
        Item item = BuscarItem(id);
        if (item == null)
        {
            Debug.LogWarning("No se encontr� el item con id: " + id);
            return false;
        }

        // Si es foto, liberar su textura antes de eliminar
        if (item.isPhoto && item.datosFoto != null)
        {
            item.datosFoto.LiberarTextura();
            Debug.Log($"Textura liberada al eliminar item: {item.itemName}");
        }

        itemsList.Remove(item);
        OnItemEliminado?.Invoke(item);

        // Ajustar �ndice si qued� fuera de rango
        if (_index >= itemsList.Count)
        {
            _index = Mathf.Max(0, itemsList.Count - 1);
        }

        if (_isInventoryOpen) UpdateUI();

        Debug.Log($"Item eliminado: {item.itemName} | Total: {itemsList.Count}/{maxItems}");
        return true;
    }

    public Item BuscarItem(int ID)
    {
        return itemsList.Find(i => i.ID == ID);
    }

    public bool InventarioLleno() => itemsList.Count >= maxItems;
    public int EspacioRestante() => maxItems - itemsList.Count;

    private void LiberarTodasLasTexturas()
    {
        foreach (Item item in itemsList)
        {
            if (item.isPhoto && item.datosFoto != null)
            {
                item.datosFoto.LiberarTextura();
                Debug.Log($"Textura liberada al cerrar inventario: {item.itemName}");
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

        // Bot�n Derecho
        _root.Q<Button>("RightButton").clicked += () =>
        {
            if (itemsList.Count == 0) return;
            _index = (_index + 1) % itemsList.Count;
            UpdateUI();
        };

        // Bot�n Izquierdo
        _root.Q<Button>("LeftButton").clicked += () =>
        {
            if (itemsList.Count == 0) return;
            _index = (_index - 1 + itemsList.Count) % itemsList.Count;
            UpdateUI();
        };

        // Bot�n Usar
        _actionButton.clicked += () =>
        {
            if (itemsList.Count == 0)
                return;

            var item = itemsList[_index];

            if (item.isUsable)
            {
                if (PLAYER.heldPhoto == null)
                {
                    Destroy(PLAYER.heldPhoto);
                    PLAYER.heldPhoto = null;
                }

                GameObject fotoInstanciada = Instantiate(item.prefabItem, PLAYER.heldPosition.position, PLAYER.heldPosition.rotation);
            }
        
        };

        // Bot�n Examinar
        _examineButton.clicked += () =>
        {
            if (itemsList.Count == 0) return;

            var item = itemsList[_index];

            GameObject inspectTarget = item.GetObjectForInspection();
            bool wantsInspect = false;

            if (inspectTarget != null)
                wantsInspect = true;

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
        if (currentItem.isPhoto && currentItem.datosFoto != null)
        {
            currentItem.datosFoto.CargarTextura();
            if (currentItem.datosFoto.textura != null)
            {
                Sprite spriteTextura = TexturaASprite(currentItem.datosFoto.textura);

                if (spriteTextura != null)
                    _bigItemImage.style.backgroundImage = new StyleBackground(spriteTextura);
                else
                    _bigItemImage.style.backgroundImage = StyleKeyword.None;
            }
        }
        else
        {
            if (currentItem.sprite != null)
                _bigItemImage.style.backgroundImage = new StyleBackground(currentItem.sprite);
            else 
                _bigItemImage.style.backgroundImage = StyleKeyword.None;
        }

        _bigItemImage.style.backgroundSize = new BackgroundSize(BackgroundSizeType.Contain);
        _labelName.text = currentItem.itemName.ToUpper();
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
        if (item.isPhoto && item.datosFoto != null)
        {
            item.datosFoto.CargarTextura();

            if (item.datosFoto.spriteCache != null)
            {
                ve.style.backgroundImage = new StyleBackground(item.datosFoto.spriteCache);
            }

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
            inspectTarget = item.GetObjectForInspection();
            if (inspectTarget == null)
            {
                Debug.Log("Null");
            }
            if (inspectTarget != null)
            {
                wantsInspect = true;
                EliminarItem(item.ID);
            }
        }
    }


    #region Save & Load
    public void SaveData(ref InventorySaveData data)
    {
        data.items = new List<ItemSaveData>(itemsList.Count);

        foreach (Item item in itemsList)
        {
            // Generar la entrada a guardar
            var entry = new ItemSaveData()
            {
                definitionID = item.ID,
                quantity = item.quantity,
                isPhoto = item.isPhoto,
                customName = item.customName,
                customDescription = item.customDescription,
            };

            // Si es foto, se guarda la informaci�n de la foto
            if (item.isPhoto && item.datosFoto != null)
            {
                // Informaci�n varia
                entry.datosFoto = new DatosFotosSaveData
                {
                    idFoto = item.datosFoto.idFoto,
                    folderRoute = item.datosFoto.folderRoute,
                    revealTime = item.datosFoto.revealTime,
                    revealProgress = item.datosFoto.revealProgress,
                    enemyIDs = new List<string>()
                };

                // lista de enemigos
                foreach (var enemy in item.datosFoto.enemiesCaught)               
                    if (enemy != null)
                        entry.datosFoto.enemyIDs.Add(enemy.gameObject.name);
                
            }

            // Guardar la entrada
            data.items.Add(entry);
        }
    }

    public void LoadData(InventorySaveData data)
    {
        itemsList.Clear();
        _index = 0;

        if (data.items == null)
            return;

        foreach (ItemSaveData savedItem in data.items)
        {
            if (!itemDatabase.TryGet(savedItem.definitionID, out ItemDefinition def))            
                continue;
            
            // Se crea un objeto nuevo
            Item item = new Item(def, savedItem.quantity);

            item.customName = savedItem.customName;
            item.customDescription = savedItem.customDescription;

            // Copiado de datos
            if (savedItem.isPhoto)
            {
                item.datosFoto = new DatosFotos(
                    savedItem.datosFoto.idFoto,
                    savedItem.datosFoto.folderRoute,
                    savedItem.datosFoto.revealTime
                );
                item.datosFoto.revealProgress = savedItem.datosFoto.revealProgress;
            }

            bool added = AgregarItem(item);

            if (!added)            
                break;     
        }
    }
    #endregion
}

[System.Serializable]
public struct InventorySaveData
{
    // Lista de objetos
    public List<ItemSaveData> items;
}

[System.Serializable]
public struct ItemSaveData
{
    // General
    public int definitionID;
    public int quantity;

    // Foto
    public bool isPhoto;
    public string customName;
    public string customDescription;
    public DatosFotosSaveData datosFoto; // Si: esFoto == true
}

[System.Serializable]
public struct DatosFotosSaveData
{
    public string idFoto;
    public string folderRoute;
    public float revealTime;
    public float revealProgress;

    public List<string> enemyIDs;
}
