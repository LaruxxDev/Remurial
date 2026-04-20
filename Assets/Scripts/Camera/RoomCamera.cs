using UnityEngine;
using Unity.Cinemachine;

public class RoomCamera : MonoBehaviour
{
    [Header("Cámara de esta habitación")]
    public CinemachineCamera roomCamera;

    [Header("Configuración de Prioridad")]
    public int activePriority = 11; 
    public int inactivePriority = 0;

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Jugador ha entrado en la zona de la cámara de la habitación.");
        // Cuando el jugador entra en la zona, activamos la cámara
        if (other.CompareTag("Player"))
        {
            roomCamera.Priority = activePriority;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando el jugador sale de la zona, apagamos la cámara
        if (other.CompareTag("Player"))
        {
            roomCamera.Priority = inactivePriority;
        }
    }
}
