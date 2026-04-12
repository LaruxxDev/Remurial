using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;
    // float snappedHorizontalMovement;
    // float snappedVerticalMovement;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        /*if (horizontalMovement > 0)
        {
            snappedHorizontalMovement = 1;
        }
        else if (horizontalMovement < 0)
        {
            snappedHorizontalMovement = -1;
        }
        else
        {
            snappedHorizontalMovement = 0;
        }*/
        
        //animator.SetFloat("Horizontal", horizontalMovement, 0.1f, Time.deltaTime);
        //animator.SetFloat("Vertical", verticalMovement, 0.1f, Time.deltaTime);
    }
}
