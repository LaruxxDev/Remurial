using UnityEngine;

public class InputTransformer
{
    public InputTransformer() { }
    private bool inputsEnabled = true;


    // Movement
    Vector2 inputVector;
    public Vector2 INPUTVECTOR => inputVector;
    public Vector2 INPUTVECTORNORMAL => inputVector.normalized;


    // Camera
    float inputCamera;
    public float INPUTCAMERA => inputCamera;


    // Interact
    float inputInteract;
    public float INPUTINTERACT => inputInteract;


    // Flash
    float inputFlash;
    public float INPUTFLASH => inputFlash;




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

    public void ProcessInputCamera(float value)
    {
        if (!inputsEnabled)
            return;

        this.inputCamera = value;
    }

    public void ProcessInputInteract(float value)
    {
        if (!inputsEnabled)
            return;

        this.inputInteract = value;
    }

    public void ProcessInputFlash(float value)
    {
        if (!inputsEnabled)
            return;

        this.inputFlash = value;
    }

}