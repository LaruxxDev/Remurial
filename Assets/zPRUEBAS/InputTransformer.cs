using UnityEngine;

public class InputTransformer
{
    public InputTransformer() { }
    private bool inputsEnabled = true;


    // Movement
    Vector2 inputVector;
    public Vector2 INPUTVECTOR => inputVector;
    public Vector2 INPUTVECTORNORMAL => inputVector.normalized;

    // Aim
    Vector2 inputAim;
    public Vector2 INPUTAIM => inputAim;
    public Vector2 INPUTAIMNORMAL => inputAim.normalized;


    // Left Click
    float LeftClickInput;
    public float LEFTCLICK => LeftClickInput;


    // Right Click
    float RightClickInput;
    public float RIGHTCLICK => RightClickInput;


    // F
    float fInput;
    public float F => fInput;


    // I
    float iInput;
    public float I => iInput;


    // B
    float bInput;
    public float B => bInput;


    // Esc
    float escapeInput;
    public float ESC => escapeInput;



    public void EnableInputs(bool areEnabled)
    {
        inputsEnabled = areEnabled;
    }


    public void ProcessInputVector(Vector2 value)
    {
        if (!inputsEnabled)
            return;

        this.inputVector = value;
    }

    public void ProcessInputAim(Vector2 value)
    {
        if (!inputsEnabled)
            return;

        this.inputAim = value;
    }


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


    public void ProcessInputEsc(float value)
    {
        this.escapeInput = value;
    }
}