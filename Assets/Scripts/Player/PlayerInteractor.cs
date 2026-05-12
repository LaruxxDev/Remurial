using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private GameInputReader _input;
    public Transform leftHand; // Asigna el transform de la mano izquierda en el inspector

    private IInteractable _currentInteractable;
    private GameObject interactuableItem; // Guardamos el objeto interactuable detectado para usarlo al interactuar
    private GameObject grabbedItem;

    private void OnEnable()  => _input.OnInteractStarted += HandleInteract;
    private void OnDisable() => _input.OnInteractStarted -= HandleInteract;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Interactuable")) return;
        interactuableItem = other.gameObject;
        if (other.tag == "Tp") interactuableItem = this.gameObject;
        if (other.TryGetComponent<IInteractable>(out var interactable))
        {
            _currentInteractable = interactable;
            Debug.Log("Detectado: " + other.name);

            if (HudManager.Instance != null)
                HudManager.Instance.MostrarMensaje(_currentInteractable.GetInteractText());
            else
                Debug.LogWarning("No se encontró el HUDManager.");
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IInteractable>(out var interactable) 
            && interactable == _currentInteractable)
        {
            _currentInteractable = null;
            interactuableItem = null; // Limpiamos la referencia al objeto interactuable al salir del área de interacción    
            // Aquí puedes ocultar el mensaje del HUD
        }
    }

    private void HandleInteract()
    {
        if (grabbedItem != null)
        {
            Debug.Log("Usando objeto agarrado: " + grabbedItem.name);
            grabbedItem.GetComponent<Tp>()?.TryOpen(grabbedItem.GetComponent<Item>()?.id ?? 0); // Reemplaza 1 con el ID real
            Destroy(grabbedItem);
        }
        if (_currentInteractable != null)
        {
            Debug.Log("Interactuando con: " + (_currentInteractable as MonoBehaviour)?.name);
            _currentInteractable.Interact(this.gameObject);
        
        }
        else
        {
            Debug.Log("No hay ningún objeto interactuable cercano.");
        }
    }

    public void AgarrarObjeto(GameObject item)
    {
        if (grabbedItem != null)
        {
            Destroy(grabbedItem);

        }
        // Desactivar física y colisiones
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null)
        {
            //rb.isKinematic = true;
            // rb.detectCollisions = false;
        }
        GameObject objetoAgarrado = Instantiate(item);
        grabbedItem = objetoAgarrado;
        // Posicionar el objeto en la mano del jugador
        objetoAgarrado.transform.SetParent(leftHand);
        objetoAgarrado.transform.localPosition = Vector3.zero;
        objetoAgarrado.transform.localRotation = Quaternion.identity; // opcional: ajustar rotación
    }


}