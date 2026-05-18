using UnityEngine;
using UnityEngine.InputSystem;

public class InputTransformer
{
    public InputTransformer(PlayerInput playerInput) 
    {
        this.playerInput = playerInput;
    }



    #region General
    private PlayerInput playerInput;

    private bool inputsEnabled = true;

    public System.Action OnInteractStarted;

    public void EnableInputs(bool areEnabled)
    {
        inputsEnabled = areEnabled;
    }


    public void ToggleInputMap(string map)
    {
        switch (map.ToLower().Trim())
        {
            case "player":
                if (playerInput.currentActionMap.name != "Player")
                    playerInput.SwitchCurrentActionMap("Player");
                break;

            case "ui":
                if (playerInput.currentActionMap.name != "UI")
                    playerInput.SwitchCurrentActionMap("UI");
                break;

            default:
                Debug.LogError("InputMap not found.");
                break;
        }
    }
    #endregion

    #region Player
    #region Vectores
    // Movement
    Vector2 inputMovement;
    public Vector2 INPUTMOVEMENT => inputMovement;
    public Vector2 INPUTMOVEMENTNORMAL => inputMovement.normalized;

    // Aim
    Vector2 inputAim;
    public Vector2 INPUTAIM => inputAim;
    public Vector2 INPUTAIMNORMAL => inputAim.normalized;


    public void ProcessInputMovement(Vector2 value)
    {
        if (!inputsEnabled)
            return;

        this.inputMovement = value;
    }


    public void ProcessInputAim(Vector2 value)
    {
        if (!inputsEnabled)
            return;

        this.inputAim = value;
    }
    #endregion

    #region Inputs Singulares
    // Left Click
    float LeftClickInput;
    public float LEFTCLICK => LeftClickInput;


    // Right Click
    float RightClickInput;
    public float RIGHTCLICK => RightClickInput;


    // F
    float fInput;
    public float F => fInput;


    // Q
    float qInput;
    public float Q => qInput;


    // G
    float gInput;
    public float G => gInput;


    // R
    float rInput;
    public float R => rInput;


    // E
    float eInput;
    public float E => eInput;


    // I
    float iInput;
    public float I => iInput;


    // B
    float bInput;
    public float B => bInput;


    // Tab
    float tabInput;
    public float TAB => tabInput;


    // Control
    float controlInput;
    public float CONTROL => controlInput;


    // Shift
    float shiftInput;
    public float SHIFT => shiftInput;


    // Esc
    float escapeInput;
    public float ESC => escapeInput;



    public void ProcessInputLeftClick(float value)
    {
        if (!inputsEnabled)
            return;

        this.LeftClickInput = value;
    }


    public void ProcessInputRightClick(float value)
    {
        if (!inputsEnabled)
            return;

        this.RightClickInput = value;
    }


    public void ProcessInputQ(float value)
    {
        if (!inputsEnabled)
            return;

        this.qInput = value;
    }


    public void ProcessInputR(float value)
    {
        if (!inputsEnabled)
            return;

        this.rInput = value;
    }


    public void ProcessInputG(float value)
    {
        if (!inputsEnabled)
            return;

        this.gInput = value;
    }


    public void ProcessInputE(float value)
    {
        if (!inputsEnabled)
            return;

        this.eInput = value;
    }


    public void ProcessInputF(float value)
    {
        if (!inputsEnabled)
            return;

        this.fInput = value;
    }


    public void ProcessInputI(float value)
    {
        if (!inputsEnabled)
            return;

        this.iInput = value;
    }


    public void ProcessInputB(float value)
    {
        if (!inputsEnabled)
            return;

        this.bInput = value;
    }


    public void ProcessInputTab(float value)
    {
        if (!inputsEnabled)
            return;

        this.tabInput = value;
    }


    public void ProcessInputControl(float value)
    {
        if (!inputsEnabled)
            return;

        this.controlInput = value;
    }


    public void ProcessInputShift(float value)
    {
        if (!inputsEnabled)
            return;

        this.shiftInput = value;
    }


    public void ProcessInputEsc(float value)
    {
        this.escapeInput = value;
    }
    #endregion
    #endregion



    #region UI
    #region Vectores
    // Movement
    Vector2 inputNavigate;
    public Vector2 INPUTNAVIGATE => inputNavigate;
    public Vector2 INPUTNAVIGATENORMAL => inputNavigate.normalized;


    public void ProcessInputNavigate(Vector2 value)
    {
        if (!inputsEnabled)
            return;

        this.inputNavigate = value;
    }
    #endregion

    #region Inputs Singulares
    // Confirm
    float confirmInput;
    public float CONFIRM => confirmInput;


    // EscDos
    float escDosInput;
    public float ESCDOS => escDosInput;

    public void ProcessInputConfirm(float value)
    {
        if (!inputsEnabled)
            return;

        this.confirmInput = value;
    }

    public void ProcessInputEscDos(float value)
    {
        if (!inputsEnabled)
            return;

        this.escDosInput = value;
    }
    #endregion
    #endregion
}