using System.Collections;
using UnityEngine;

public class BlockedDoor : MonoBehaviour, IInteractable
{
    [Header("Dialogues")]
    [SerializeField] private GameObject dialogueClosed;
    [SerializeField] private GameObject dialogueOpen;
    [SerializeField] private bool isOpen = false;

    public bool isInspectable => false;

    public void Interact(GameObject interactor)
    {
        // Temporalmente aquí, moverlo a donde se abra la puerta tras usar el objeto
        if (!isOpen)
        {
            dialogueOpen.SetActive(false);

            dialogueClosed.GetComponent<DialogueTrigger>().CallDialogue();
        }
        else
        {
            dialogueOpen.SetActive(true);
            dialogueClosed.SetActive(false);

            dialogueOpen.GetComponent<DialogueTrigger>().CallDialogue();
        }
    }

    public bool UseItem(GameObject item)
    {
        //var pickup = item.GetComponent<PickupInteractable>();
        //int itemId = pickup?.Definition?.ID ?? 0;
        //Debug.Log($"Intentando abrir TP con ID: {itemId}");
        //if (itemId == IdDoor && !isOpen)
        //{
        //    InventoryManager.Instance.EliminarItem(itemId); // Eliminar el item del inventario al usarlo
        //    isOpen = true;
        //    return true;
        //}
        return false;
    }


    public string GetInteractText() => "Puerta cerrada";
}