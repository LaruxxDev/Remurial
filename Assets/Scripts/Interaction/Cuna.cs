using System.Collections;
using UnityEngine;

public class Cuna : MonoBehaviour, IInteractable
{
    public bool isCorrect = false;
    public float tiempoInterpolacion = 1.0f;
    [SerializeField] private int IdCuna = 2;
    [SerializeField] private Transform destino;
    public void Interact(GameObject interactor)
    {
        return;
    }



    public bool UseItem(GameObject item)
    {
        if (isCorrect) return false; // Si ya se ha usado el item correcto, no hacer nada.
        var pickup = item.GetComponent<PickupInteractable>();
        int itemId = pickup?.Data?.id ?? 0;
        Debug.Log($"Intentando colocar el item en la Cuna con ID: {itemId}");
        if (itemId == IdCuna && !isCorrect)
        {
            Debug.Log("Item correcto usado en la cuna.");
            item.GetComponent<Collider>().enabled = false; // Desactivar colisión para evitar problemas durante la interpolación
            interpolarCuna(item);
            
            isCorrect = true;
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