using UnityEngine;
using UnityEngine.InputSystem;
public class InputManager : MonoBehaviour
{
    float horizontalMovement;
    float verticalMovement;
    AnimatorManager animatorManager;

    private void Awake()
    {
        animatorManager = GetComponent<AnimatorManager>();
    }

    private void Update()
    {

        if (Keyboard.current != null)
        {
            if (Keyboard.current.aKey.isPressed || Keyboard.current.leftArrowKey.isPressed) horizontalMovement = -1;
            else if (Keyboard.current.dKey.isPressed || Keyboard.current.rightArrowKey.isPressed) horizontalMovement = 1;
            else horizontalMovement = 0;

            if (Keyboard.current.wKey.isPressed || Keyboard.current.upArrowKey.isPressed) verticalMovement = 1;
            else if (Keyboard.current.sKey.isPressed || Keyboard.current.downArrowKey.isPressed) verticalMovement = -1;
            else verticalMovement = 0;
        }

        animatorManager.HandleAnimatorValues(horizontalMovement, verticalMovement);
    }   
}