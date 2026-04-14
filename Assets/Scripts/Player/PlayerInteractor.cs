using UnityEngine;

public class PlayerInteractor : MonoBehaviour
{
    [SerializeField] private GameInputReader _input;

    private IInteractable _currentInteractable;
    private GameObject interactuableItem; // Guardamos el objeto interactuable detectado para usarlo al interactuar

    private void OnEnable()  => _input.OnInteractStarted += HandleInteract;
    private void OnDisable() => _input.OnInteractStarted -= HandleInteract;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer != LayerMask.NameToLayer("Interactuable")) return;
        interactuableItem = other.gameObject;
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
        if (_currentInteractable != null)
        {
            Debug.Log("Interactuando con: " + (_currentInteractable as MonoBehaviour)?.name);
            _currentInteractable.Interact(interactuableItem);
        }
        else
        {
            Debug.Log("No hay ningún objeto interactuable cercano.");
        }
    }
}