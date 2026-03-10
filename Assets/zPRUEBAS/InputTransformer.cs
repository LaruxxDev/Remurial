using UnityEngine;

public class InputTransformer
{
    public InputTransformer() { }
    private bool inputsEnabled = true;


    // Movement
    Vector2 inputVector;
    public Vector2 INPUTVECTOR => inputVector;
    public Vector2 INPUTVECTORNORMAL => inputVector.normalized;
    
    
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
}