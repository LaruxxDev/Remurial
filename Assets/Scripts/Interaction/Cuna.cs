using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Cuna : MonoBehaviour, IInteractable
{
    [Header("References")]
    [SerializeField] private Transform destino;
    [SerializeField] private PlayerGeneral PLAYER;

    private GameObject itemEnCuna;

    [Header("Variables")]
    [SerializeField] private int IdCuna = 2;
    public bool isEmpty = true;
    public bool isCorrect = false;
    public float tiempoInterpolacion = 1.0f;

    // Cambiarlos por textos localizables
    [SerializeField] private string descriptionEmpty;   // "Usar objeto en la cuna"
    [SerializeField] private string descriptionFull;    // "Recoger objeto de la cuna"


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

            Destroy(itemEnCuna);
            isCorrect = false;
            isEmpty = true;
            itemEnCuna = null;
            Debug.Log("Cuna viva Item cuna distinto null");

            return;
        }

        // CASO B: La cuna está vacía. Buscamos el objeto que el jugador tiene en la mano real.
        if (PLAYER != null)
        {
            // Buscamos si hay algo en la mano (heldPosition)
            if (PLAYER.heldPosition != null && PLAYER.heldPosition.childCount > 0)
            {
                //GameObject objetoEnMano = PLAYER.heldPosition.GetChild(0).gameObject;
                GameObject objetoEnMano = PLAYER.heldItem;

                Debug.Log($"Detectado objeto en mano: {objetoEnMano.name}");

                if (UseItem(objetoEnMano))
                {
                    // Limpiamos la mano del jugador
                    PLAYER.heldItem = null;

                    GameManager.Instancia.TrySolvePuzzle();

                    return;
                }
            }
        }
    }


    public bool UseItem(GameObject item)
    {
        if (!isEmpty)
            return false; // Si ya se ha usado el item correcto, no hacer nada


        var pickup = item.GetComponent<PickupInteractable>();

        // DEBUG: Vamos a ver qué está pasando exactamente
        if (pickup == null)
        {
            pickup = item.GetComponentInChildren<PickupInteractable>();
            Debug.LogError("¡El objeto no tiene PickupInteractable!");
            return false;
        }

        if (pickup.Definition == null)
        {
            Debug.LogError($"¡El objeto {item.name} tiene el script pero la Definition es NULL!");
            return false;
        }


        int itemId = pickup.Definition != null ? pickup.Definition.ID : 0;
        Debug.Log($"Intentando colocar objeto ID: {itemId}. ID de la Cuna requerida: {IdCuna}");

        if (isEmpty)
        {
            if (itemId == IdCuna)
                isCorrect = true;

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

            isEmpty = false;
            itemEnCuna = item;

            return true;
        }

        return false;
    }

    // Mover objeto a la cuna
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

        AudioManager.instance.Play3D("RecogerItem", destino.position);

        item.transform.SetParent(this.transform);
        item.transform.position = destino.position; // Asegurar que llegue exactamente al destino al final
    }

    public string GetInteractText() => isEmpty ? descriptionEmpty : descriptionFull;
}