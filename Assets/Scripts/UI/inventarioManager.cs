using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UIElements;

public class InventarioManager : MonoBehaviour
{
    
    public List<Item> itemsList;
    private int _index = 0;

    private VisualElement _carrusel;

    private VisualElement _bigItemImage;
    private Label _labelName;
    private Label _labelDesc;
    private Button _actionButton;



    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _carrusel = root.Q<VisualElement>("Carrusel");

        _labelName = root.Q<Label>("ItemName");
        _bigItemImage = root.Q<VisualElement>("ItemImage");
        _labelDesc = root.Q<Label>("ItemDesc");
        _actionButton = root.Q<Button>("ActionButton");

        //Navegació teclad y mando
        root.RegisterCallback<NavigationMoveEvent>(evt =>
        {
            if (evt.direction == NavigationMoveEvent.Direction.Right)
            {
                _index++;
                if (_index >= itemsList.Count)
                    _index = 0;
                UpdateUI();
            }
            else if (evt.direction == NavigationMoveEvent.Direction.Left)
            {
                _index--;
                if (_index < 0)
                    _index = itemsList.Count - 1;
                UpdateUI();
            }
        });


        //Navegación con botones
        root.Q<Button>("RightButton").clicked += () =>
        {
            _index++;
            if (_index >= itemsList.Count)
                _index = 0;
            UpdateUI();
        };

        root.Q<Button>("LeftButton").clicked += () =>
        {
            _index--;
            if (_index < 0)
                _index = itemsList.Count - 1;
            UpdateUI();       
        };

        //TODO: Acción del botón
        _actionButton.clicked += () =>
        {
            var item = itemsList[_index];
            if (item.isUsable)
            {
                _actionButton.text = "USE";
                Debug.Log($"Usando {item.name}");
                // Aquí puedes agregar la lógica para usar el item
            }else
            {
                _actionButton.text = "NO USABLE";
                Debug.Log($"{item.name} no es usable");
            }
        };

        root.Focus();

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (itemsList.Count == 0)
        {
            _labelName.text = "No items";
            _labelDesc.text = "";
            _bigItemImage.style.backgroundImage = null;
            _actionButton.SetEnabled(false);
            return;
        }

        var item = itemsList[_index];
        _labelName.text = item.name;
        _labelDesc.text = item.description;
        _bigItemImage.style.backgroundImage = new StyleBackground(item.sprite);
        _actionButton.SetEnabled(item.isUsable);
        _actionButton.text = item.isUsable ? "USE" : "NO USABLE";
    }
    

}
