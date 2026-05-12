using UnityEngine;
public interface IInteractable
{
    void Interact(GameObject interactor);
    bool UseItem(int id); // Para usar un item específico en el objeto interactuable, si es necesario.
    string GetInteractText(); // Para mostrar "Recoger X" o "Abrir puerta", etc.
} 