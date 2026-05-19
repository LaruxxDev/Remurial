using System.Collections;
using UnityEngine;

public class Cuna : MonoBehaviour, IInteractable
{
    public bool isCorrect = false;
    public float tiempoInterpolacion = 1.0f;
    [SerializeField] private int IdCuna = 2;
    [SerializeField] private Transform destino;
    [SerializeField] private PlayerGeneral PLAYER;


    private GameObject itemEnCuna;

    public bool isInspectable => false;

    public void Interact(GameObject interactor)
    {
        Debug.Log("Cuna viva");
        // CASO A: Si ya hay un item colocado, lo recogemos
        if (itemEnCuna != null)
        {
            if (itemEnCuna.TryGetComponent<PickupInteractable>(out var pickup))
            {
                pickup.Interact(interactor);
            }
            isCorrect = false;
            itemEnCuna = null;
            Debug.Log("Cuna viva Item cuna distinto null");

            return;
        }

        // CASO B: La cuna está vacía. Buscamos el objeto que el jugador tiene en la mano real.
        if (PLAYER != null)
        {
            Debug.Log("aa1");

            // Buscamos si hay algo en la mano (heldPosition)
            if (PLAYER.heldPosition != null && PLAYER.heldPosition.childCount > 0)
            {
                GameObject objetoEnMano = PLAYER.heldPosition.GetChild(0).gameObject;
                Debug.Log($"Detectado objeto en mano: {objetoEnMano.name}");

                if (UseItem(objetoEnMano))
                {
                    Debug.Log("bbb1");
                    // Limpiamos la mano del jugador
                    PLAYER.heldItem = null;
                    Destroy(objetoEnMano);
                    return;
                }
            }
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
            // Eliminamos el item del inventario lógico
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.EliminarItem(itemId); 
            }

            // Desactivamos colisiones para que no choque con el jugador mientras vuela a la cuna
            if (item.TryGetComponent<Collider>(out var col)) 
            {
                col.enabled = false; 
            }
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