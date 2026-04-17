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
        if (context.performed)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputInteract(1f);
        }

        if (context.canceled)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputInteract(0f);
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

    public void InputAim(InputAction.CallbackContext context)
    {
        if (!inputsEnabled)
            return;

        // Movement

        if (context.canceled)
            PLAYER.INPUTTRANSFORMER.ProcessInputAim(Vector2.zero);
        else
            PLAYER.INPUTTRANSFORMER.ProcessInputAim(context.ReadValue<Vector2>());
    }
    #endregion

    #region UI
    public void InputPause(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Pause

            //Debug.Log("Pause");
            //if (!GameManager.instance.gameEnded)
            //    GameManager.instance.TogglePause("pause");



            // Temporal: Save
            Debug.Log("Save");

            //SaveSystem.Save();

            //PLAYER.CONFIGURATION.SaveData();
        }
    }

    public void InputPrueba(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            // Temporal: Load
            Debug.Log("Load");

            SaveSystem.Load();

            //PLAYER.CONFIGURATION.LoadData();
        }
    }
    #endregion
}
