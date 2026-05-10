using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField] private Animator animator;


    //public void HandleAnimatorValues(float horizontalMovement, float verticalMovement)
    //{
    //    // Redondeamos valores muy pequeños a 0
    //    float speed = Mathf.Round(verticalMovement * 100f) / 100f; // Redondea a 2 decimales

    //    animator.SetFloat("Speed", speed, 0.1f, Time.deltaTime);
    //}


    public void HandleAnimatorValues(Rigidbody rb)
    {
        Transform transform = rb.transform;
        Vector3 localVelocity = transform.InverseTransformDirection(rb.linearVelocity);

        //float horizontal = localVelocity.x;
        float vertical = localVelocity.z;

        //animator.SetFloat("Horizontal", horizontal, 0.15f, Time.deltaTime);
        animator.SetFloat("Speed", vertical, 0.15f, Time.deltaTime);
    }



    public void PlayAnimation(string stateName, float crossFade = 0.2f)
    {
        animator.CrossFadeInFixedTime(stateName, crossFade);
    }
}
