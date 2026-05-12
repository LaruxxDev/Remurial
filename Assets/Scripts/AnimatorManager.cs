using UnityEngine;

public class AnimatorManager : MonoBehaviour
{
    [SerializeField] private Animator animator;

    private Vector2 _smoothVelocity;

    [SerializeField][Range(1f, 15f)] private float accelerationSmooth = 1f;
    [SerializeField][Range(1f, 15f)] private float decelerationSmooth = 7f;

    public float speedModifier;

    public void HandleAnimatorValues(Rigidbody rb, float moveSpeed)
    {
        Vector3 localVelocity = rb.transform.InverseTransformDirection(rb.linearVelocity);
        Vector2 targetVelocity = new Vector2(localVelocity.x / moveSpeed, localVelocity.z / moveSpeed);

        float lerpSpeed = targetVelocity.magnitude > _smoothVelocity.magnitude ? accelerationSmooth : decelerationSmooth;
        _smoothVelocity = Vector2.Lerp(_smoothVelocity, targetVelocity, lerpSpeed * Time.fixedDeltaTime);

        float normalizedSpeed = Mathf.Abs(_smoothVelocity.y);
        animator.SetFloat("Speed", normalizedSpeed, 0f, Time.fixedDeltaTime);
        animator.speed = Mathf.Lerp(0f, 1f, normalizedSpeed) * speedModifier;

        float targetMotionSpeed = _smoothVelocity.y < -0.01f ? -1f : 1f;
        animator.SetFloat("MotionSpeed", targetMotionSpeed, 0.1f, Time.fixedDeltaTime);
    }



    public void PlayAnimation(string stateName, float crossFade = 0.2f)
    {
        animator.CrossFadeInFixedTime(stateName, crossFade);
    }
}
