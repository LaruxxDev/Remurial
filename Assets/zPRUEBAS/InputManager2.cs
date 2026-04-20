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
    // Movement
    public void InputMovement(InputAction.CallbackContext context)
    {
        if (!inputsEnabled)
            return;

        if (context.canceled)
            PLAYER.INPUTTRANSFORMER.ProcessInputVector(Vector2.zero);
        else
            PLAYER.INPUTTRANSFORMER.ProcessInputVector(context.ReadValue<Vector2>());
    }

    // Aim
    public void InputAim(InputAction.CallbackContext context)
    {
        if (!inputsEnabled)
            return;

        if (context.canceled)
            PLAYER.INPUTTRANSFORMER.ProcessInputAim(Vector2.zero);
        else
            PLAYER.INPUTTRANSFORMER.ProcessInputAim(context.ReadValue<Vector2>());
    }


    // Left Click
    public void InputLeftClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputLeftClick(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputLeftClick(0f);
        }
    }

    // Right Click
    public void InputRightClick(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputRightClick(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputRightClick(0f);
        }
    }

    // F
    public void InputF(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputF(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputF(0f);
        }
    }

    // I
    public void InputI(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputI(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputI(0f);
        }
    }

    // B
    public void InputB(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputB(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputB(0f);
        }
    }
    #endregion

    #region UI
    // Escape
    public void InputEsc(InputAction.CallbackContext context)
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
