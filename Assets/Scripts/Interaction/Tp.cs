using System.Collections;
using UnityEngine;

public class Tp : MonoBehaviour, IInteractable
{
    [SerializeField] private Transform destino;
    [SerializeField] private float tiempoEnNegro = 1f;
    [SerializeField] private float cooldownPuertaCerrada = 2f; // Tiempo entre sonidos

    private bool estaTeletransportando = false;
    private bool enCooldownPuertaCerrada = false; 

    public bool isOpen = true;
    public int IdDoor = 1;

    public bool isInspectable => false;

    public void Interact(GameObject interactor)
    {
        if (!isOpen)
        {
            if (enCooldownPuertaCerrada) return; 

            StartCoroutine(CooldownPuertaCerrada());
            AudioManager.instance.Play3D("ClosedDoor", transform.position);
            return; 
        }

        if (estaTeletransportando) return;
        StartCoroutine(ProcesoTeletransporte(interactor));
    }

    private IEnumerator CooldownPuertaCerrada()
    {
        enCooldownPuertaCerrada = true;
        yield return new WaitForSeconds(cooldownPuertaCerrada);
        enCooldownPuertaCerrada = false;
    }

    public bool UseItem(GameObject item)
    {
        var pickup = item.GetComponent<PickupInteractable>();
        int itemId = pickup?.Data?.id ?? 0;
        Debug.Log($"Intentando abrir TP con ID: {itemId}");
        if (itemId == IdDoor && !isOpen)
        {
            InventarioManager.Instance.EliminarItem(itemId); // Eliminar el item del inventario al usarlo
            isOpen = true;
            return true;
        }
        return false;
    }

    private IEnumerator ProcesoTeletransporte(GameObject interactor)
    {
        estaTeletransportando = true;
        AudioManager.instance.Play3D("OpenDoor", transform.position);

        GameManager.Instancia.HacerBlackout();
        yield return new WaitForSeconds(1.0f);

        interactor.transform.position = destino.position;
        yield return new WaitForSeconds(tiempoEnNegro);

        GameManager.Instancia.QuitarBlackout();
        estaTeletransportando = false;
    }

    public string GetInteractText() => isOpen ? $"Teletransportarse a {destino.name}" : "Puerta cerrada";
}