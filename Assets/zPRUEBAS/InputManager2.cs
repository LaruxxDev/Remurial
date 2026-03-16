using UnityEngine;
using UnityEngine.InputSystem;


// BORRAR // BORRAR // BORRAR // BORRAR // BORRAR // BORRAR
using Unity.Cinemachine;
// BORRAR // BORRAR // BORRAR // BORRAR // BORRAR // BORRAR

public class InputManager2 : MonoBehaviour
{
    [SerializeField] PlayerGeneral PLAYER;

    private bool inputsEnabled = true;


    public void EnableInputs(bool areEnabled)
    {
        inputsEnabled = areEnabled;
    }

    #region Gameplay
    public void InputMovement(InputAction.CallbackContext context)
    {
        if (!inputsEnabled)
            return;

        // Movement

        if (context.canceled)
            PLAYER.INPUTTRANSFORMER.ProcessInputVector(Vector2.zero);
        else
            PLAYER.INPUTTRANSFORMER.ProcessInputVector(context.ReadValue<Vector2>());
    }

    public void InputCameraSwap(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Swap Camera

            Debug.Log("Swap Camera");
        }
    }



    // BORRAR // BORRAR // BORRAR // BORRAR // BORRAR // BORRAR
    public CinemachineCamera thirdPersonCamera;
    public CinemachineCamera firstPersonCamera;
    // BORRAR // BORRAR // BORRAR // BORRAR // BORRAR // BORRAR

    public void InputInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Interact

            CameraManager.SwitchCamera(firstPersonCamera);

            Debug.Log("Interact");
        }
    }

    public void InputFlash(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Flash

            CameraManager.SwitchCamera(thirdPersonCamera);

            Debug.Log("Flash");
        }
    }
    #endregion

    #region UI
    public void InputPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Pause

            Debug.Log("Pause");
            //if (!GameManager.instance.gameEnded)
            //    GameManager.instance.TogglePause("pause");
        }
    }
    #endregion
}
