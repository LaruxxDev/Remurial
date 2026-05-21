using System.Security.Cryptography.X509Certificates;
using UnityEngine;
public class PickupInteractable  : MonoBehaviour, IInteractable
{
    [SerializeField] private ItemDefinition definition;
    public ItemDefinition Definition => definition;
    [SerializeField] private int quantity = 1;
    [SerializeField] private InspectSystem inspectSystem;


    public bool isInspectable => false;

    public void Interact(GameObject interactor)
    {
        

        Item item = new Item(definition, quantity);

        bool picked = InventoryManager.Instance.AgregarItem(item);

        if (picked)  
        {
            AudioManager.instance.Play2D("RecogerItem");
            Debug.Log($"Recogiste: {definition.itemName}");
            PlayerCollision collision = interactor.GetComponentInParent<PlayerCollision>();
            if (collision == null) collision = FindAnyObjectByType<PlayerCollision>();
            collision?.ClearInteractable();
            Destroy(gameObject);

        }
    }

    public string GetInteractText() => $"Recoger {definition.itemName}";

    public bool UseItem(GameObject item)
    {
        return false;
    }
}