using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void HandleAnimatorValues(float horizontalMovement, float verticalMovement)
    {
        // Redondeamos valores muy pequeños a 0
        float speed = Mathf.Round(verticalMovement * 100f) / 100f; // Redondea a 2 decimales

        animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    }
}
