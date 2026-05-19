using System.Collections;
using UnityEngine;

public class Cuna : MonoBehaviour, IInteractable
{
    public bool isCorrect = false;
    public float tiempoInterpolacion = 1.0f;
    [SerializeField] private int IdCuna = 2;
    [SerializeField] private Transform destino;


    private GameObject itemEnCuna;

    public bool isInspectable => false;

    public void Interact(GameObject interactor)
    {
        Debug.Log("Cuna viva");
        if ( itemEnCuna != null)
        {
            itemEnCuna.GetComponent<PickupInteractable>().Interact(interactor); // Permitir recoger el item de la cuna
             isCorrect = false; // Resetear el estado para permitir colocar otro item
             itemEnCuna = null; // Limpiar la referencia al item en la cuna
        }
    }



    public bool UseItem(GameObject item)
    {
        if (isCorrect) return false; // Si ya se ha usado el item correcto, no hacer nada.
        var pickup = item.GetComponent<PickupInteractable>();
        int itemId = pickup?.Definition?.ID ?? 0;
        Debug.Log($"Intentando colocar el item en la Cuna con ID: {itemId}");
        if (itemId == IdCuna && !isCorrect)
        {
            Debug.Log("Item correcto usado en la cuna.");
            InventoryManager.Instance.EliminarItem(itemId); // Eliminar el item del inventario al usarlo
            item.GetComponent<Collider>().enabled = false; // Desactivar colisión para evitar problemas durante la interpolación
            interpolarCuna(item);
            
            isCorrect = true;
            itemEnCuna = item;
            return true;
        }
        return false;
    }
    private void interpolarCuna(GameObject item)
    {
        StartCoroutine(ProcesoInterpolacion(item));
    }
    private IEnumerator ProcesoInterpolacion(GameObject item)
    {
        float elapsedTime = 0f;
        Vector3 startingPos = item.transform.position;

        while (elapsedTime < tiempoInterpolacion)
        {
            item.transform.position = Vector3.Lerp(startingPos, destino.position, elapsedTime / tiempoInterpolacion);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        item.transform.SetParent(this.transform); 
        AudioManager.instance.Play3D("RecogerItem",destino.position);
        item.transform.position = destino.position; // Asegurar que llegue exactamente al destino al final
    }

    public string GetInteractText() => isCorrect ? "Recoger objeto de la cuna" : "Usar objeto en la cuna";
}