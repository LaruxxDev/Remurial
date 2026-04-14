using UnityEngine;
public interface IInteractable
{
    void Interact(GameObject interactor);
    string GetInteractText(); // Para mostrar "Recoger X" o "Abrir puerta", etc.
} 