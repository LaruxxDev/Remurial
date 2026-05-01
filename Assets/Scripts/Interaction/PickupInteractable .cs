using UnityEngine;
public class PickupInteractable  : MonoBehaviour, IInteractable
{
    [SerializeField] private Item _data;
    [SerializeField] private InspectSystem _inspectSystem;
    public Item Data => _data;

    public bool isInspectable => true;

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Recogiste: {_data.name}");

        AudioManager.instance.Play2D("RecogerItem");

        InventoryManager.Instance.AgregarItem(_data);
    }

    public string GetInteractText() => $"Recoger {_data.name}";
}