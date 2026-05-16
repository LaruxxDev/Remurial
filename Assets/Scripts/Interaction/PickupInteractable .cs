using UnityEngine;
public class PickupInteractable  : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition definition;
    [SerializeField] private int quantity = 1;
    [SerializeField] private InspectSystem inspectSystem;


    public bool isInspectable => true;

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Recogiste: {definition.itemName}");

        AudioManager.instance.Play2D("RecogerItem");

        Item item = new Item(definition, quantity);

        bool picked = InventoryManager.Instance.AgregarItem(item);

        if (picked)
            Destroy(gameObject);
    }

    public string GetInteractText() => $"Recoger {definition.itemName}";
}