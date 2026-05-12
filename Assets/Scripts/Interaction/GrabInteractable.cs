using System.Linq;
using UnityEngine;

public class GrabInteractable : MonoBehaviour, IInteractable
{
    [Header("Configuración")]
    public string playerTag = "Player";
    [SerializeField] private Item _data;
    private GameObject _player;
    [SerializeField] private InspectSystem _inspectSystem;

    public Item Data => _data;
    private bool _agarrado = false;

    public void Interact(GameObject interactor)
    {
        if (_agarrado) return;

        Transform leftHand = interactor.GetComponent<PlayerInteractor>().leftHand;
        if (leftHand == null)
        {
            Debug.LogWarning("No se encontró 'LeftHand'");
            return;
        }

        Debug.Log($"Recogiste: {_data.name}");
        AgarrarObjeto(leftHand);
        _inspectSystem.EnterInspectionMode(this.gameObject);
        
        AudioManager.instance.Play2D("RecogerItem");
    }
    public bool UseItem(int id)
    {
        // No se puede usar un item agarrado, solo interactuar para recogerlo.
        return false;
    }
    private void AgarrarObjeto(Transform playerTransform)
    {
        _agarrado = true; 
        // Desactivar física y colisiones
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.isKinematic = true;
            rb.detectCollisions = false;
        }

        // Posicionar el objeto en la mano del jugador
        transform.SetParent(playerTransform);
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.identity; // opcional: ajustar rotación
    }
    public string GetInteractText() => $"Recoger {_data.name}";

}
