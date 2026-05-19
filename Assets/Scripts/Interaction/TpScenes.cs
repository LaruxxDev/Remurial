using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TpScenes : MonoBehaviour, IInteractable
{
    [SerializeField] private string sceneDestino;
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
        int itemId = pickup?.Definition?.ID ?? 0;
        Debug.Log($"Intentando abrir TP con ID: {itemId}");
        if (itemId == IdDoor && !isOpen)
        {
            InventoryManager.Instance.EliminarItem(itemId); // Eliminar el item del inventario al usarlo
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

        ChangeScene();
        yield return new WaitForSeconds(tiempoEnNegro);

        GameManager.Instancia.QuitarBlackout();
        estaTeletransportando = false;
    }

    private void ChangeScene()
    {
        if (string.IsNullOrWhiteSpace(sceneDestino))
        {
            Debug.LogError("TpScenes: sceneDestino no está configurada.");
            return;
        }

        SceneManager.LoadScene(sceneDestino);
    }

    public string GetInteractText() => isOpen ? $"Teletransportarse a {sceneDestino}" : "Puerta cerrada";
}