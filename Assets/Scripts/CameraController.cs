using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraWeaponController : MonoBehaviour
{
    [Header("Cámaras")]
    public CinemachineCamera aimCamera; 
    [SerializeField] private InputActionReference attackAction; 
    [SerializeField] private InputActionReference aimAction; 


    [Header("Configuración")]
    public int aimPriority = 20; // Prioridad alta al apuntar
    public int defaultPriority = 9; // Prioridad baja al dejar de apuntar

    void Update()
    {
        if (aimAction.action.WasPressedThisFrame())
        {
            aimCamera.Priority = aimPriority;
        }
        else if (aimAction.action.WasReleasedThisFrame())
        {
            aimCamera.Priority = defaultPriority;
        }
        
        if (attackAction.action.WasPressedThisFrame() && aimCamera.Priority == aimPriority)
        {
            TomarFoto();
        }
    }

    void TomarFoto()
    {
        Debug.Log("¡Flash! Foto tomada.");
    }
}