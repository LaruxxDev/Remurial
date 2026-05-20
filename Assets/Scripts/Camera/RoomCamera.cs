using UnityEngine;
using Unity.Cinemachine;

public class RoomCamera : MonoBehaviour
{
    [Header("Cámara de esta habitación")]
    public CinemachineCamera roomCamera;
    private CinemachineCamera activeCamera;
    [Header("Configuración de Prioridad")]
    public int activePriority = 11; 
    public int inactivePriority = 0;

    private void OnTriggerEnter(Collider other)
    {
        // Cuando el jugador entra en la zona, activamos la cámara
        if (other.CompareTag("Player"))
        {
            PlayerGeneral player = other.transform.parent.GetComponentInChildren<PlayerGeneral>();
            if (player == null)
            {
                Debug.LogError("El objeto con tag 'Player' no tiene un componente PlayerGeneral.");
                return;
            }
            activeCamera = player.thirdPersonCamera; // Guardamos la cámara actual activa
            roomCamera.Follow = other.transform; // Aseguramos que la cámara siga al jugador
            CameraManager.SwitchCamera(roomCamera, CameraManager.BlendStyle.Instant, 0f); 
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Cuando el jugador sale de la zona, apagamos la cámara
        if (other.CompareTag("Player"))
        {
            roomCamera.Follow = null; // Dejamos de seguir al jugador
            roomCamera.Priority = inactivePriority;
            CameraManager.SwitchCamera(activeCamera, CameraManager.BlendStyle.Instant, 0f); 
        }
    }
}
