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
        Debug.Log($"Guardando item en inventario: {Data.name}"+Data.prefabItem);
        
        InventarioManager.Instance.AgregarItem(Data);
    }
    public bool UseItem(int id)
    {
        // No se puede usar un item recogido, solo interactuar para recogerlo.
        return false;
    }
    public string GetInteractText() => $"Recoger {Data.name}";
}