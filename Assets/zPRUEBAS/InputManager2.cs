using UnityEngine;
using UnityEngine.InputSystem;


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
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputCamera(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputCamera(0f);
        }
    }


    public void InputInteract(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Interact

            Debug.Log("Interact");
        }
    }


    public void InputFlash(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputFlash(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputFlash(0f);
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
