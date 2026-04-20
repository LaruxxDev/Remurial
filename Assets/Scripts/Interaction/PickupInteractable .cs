using UnityEngine;
public class PickupInteractable  : MonoBehaviour, IInteractable
{
    [SerializeField] private Item _data;
    [SerializeField] private InspectSystem _inspectSystem;
    public Item Data => _data;

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Recogiste: {_data.name}");
        _inspectSystem.EnterInspectionMode(interactor);
        AudioManager.instance.Play2D("RecogerItem");

        InventarioManager.Instance.AgregarItem(_data);
        Destroy(gameObject);
    }

    public string GetInteractText() => $"Recoger {_data.name}";
}