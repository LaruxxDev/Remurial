using UnityEngine;
public class PickupInteractable  : MonoBehaviour, IInteractable
{
    [SerializeField] private Item _data;
    [SerializeField] private InspectSystem _inspectSystem;
    public Item Data => _data;

    public void Interact(GameObject interactor)
    {
        Debug.Log($"Recogiste: {Data.name}");
        _inspectSystem.EnterInspectionMode(this.gameObject);
        AudioManager.instance.Play2D("RecogerItem");

        GuardarItemEnInventario();
        Destroy(gameObject);
    }
    private void GuardarItemEnInventario()
    {
        Item item = new Item
        {
            name = Data.name,
            description = Data.description,
            id = Data.id,
            isKeyItem = Data.isKeyItem,
            isUsable = Data.isUsable,
            quantity = Data.quantity,
            maxStack = Data.maxStack,
            sprite = Data.sprite,
            esFoto = Data.esFoto,
            datosFoto = Data.datosFoto,
            prefabItem = Data.prefabItem
        };
        Debug.Log($"Guardando item en inventario: {item.name}"+item.prefabItem);
        
        InventarioManager.Instance.AgregarItem(item);
    }
    public void UseItem(GameObject interactor)
    {
        // No se puede usar un item recogido, solo interactuar para recogerlo.
    }
    public string GetInteractText() => $"Recoger {Data.name}";
}