using UnityEngine;
using UnityEngine.InputSystem;


public class InputManager : MonoBehaviour
{
    [SerializeField] PlayerGeneral PLAYER;

    private bool inputsEnabled = true;


    public void EnableInputs(bool areEnabled)
    {
        inputsEnabled = areEnabled;
    }

    #region Player
    #region Vectores
    // Movement
    public void InputMovement(InputAction.CallbackContext context)
    {
        if (!inputsEnabled)
            return;

        if (context.canceled)
            PLAYER.INPUTTRANSFORMER.ProcessInputMovement(Vector2.zero);
        else
            PLAYER.INPUTTRANSFORMER.ProcessInputMovement(context.ReadValue<Vector2>());
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
    #endregion


    #region Input Singulares
    // Left Click
    public void InputLeftClick(InputAction.CallbackContext context)
    {
        if (context.started)
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


    // G
    public void InputG(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputG(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputG(0f);
        }
    }


    // R
    public void InputR(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputR(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputR(0f);
        }
    }


    // Q
    public void InputQ(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputQ(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputQ(0f);
        }
    }


    // E
    public void InputE(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputE(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputE(0f);
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

    // Tab
    public void InputTab(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputTab(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputTab(0f);
        }
    }

    // Control
    public void InputControl(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputControl(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputControl(0f);
        }
    }

    // Shift
    public void InputShift(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputShift(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputShift(0f);
        }
    }

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
    #endregion

    #region UI
    #region Vectores
    // Navegar
    public void InputNavigate(InputAction.CallbackContext context)
    {
        if (!inputsEnabled)
            return;

        if (context.canceled)
            PLAYER.INPUTTRANSFORMER.ProcessInputNavigate(Vector2.zero);
        else
            PLAYER.INPUTTRANSFORMER.ProcessInputNavigate(context.ReadValue<Vector2>());
    }
    #endregion


    #region Input Singulares
    // Confirm
    public void InputConfirm(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputConfirm(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputConfirm(0f);
        }
    }

    // Deny
    public void InputDeny(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputDeny(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputDeny(0f);
        }
    }
    #endregion
    #endregion
}
