using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class EnemyConfiguration : MonoBehaviour
{
    [Header("Configuration")]

    #region Wander
    [Header("Wandering")]
    [SerializeField] float wanderRadius;
    public float WANDERRADIUS => wanderRadius;

    [SerializeField] float wanderSpeed;
    public float WANDERSPEED => wanderSpeed;

    [SerializeField] float wanderTurnSpeed;
    public float WANDERTURNSPEED => wanderTurnSpeed;

    [SerializeField] float waitTime;
    public float WAITTIME => waitTime;

    public bool canWander;



    public void ResetWander()
    {
        StartCoroutine(ResetWanderRoutine(waitTime));
    }

    private IEnumerator ResetWanderRoutine(float delay)
    {
        canWander = false;

        yield return new WaitForSeconds(delay);

        canWander = true;
    }
    #endregion

    #region Follow
    [Header("Follow")]
    [SerializeField] float followSpeed;
    public float FOLLOWSPEED => followSpeed;

    [SerializeField] float turnSpeed;
    public float TURNSPEED => turnSpeed;
    #endregion

    #region Rotations
    private Quaternion horizontalRotation;
    [HideInInspector] public Quaternion HORIZONTAL => horizontalRotation;

    private Quaternion invertedRotation;
    [HideInInspector] public Quaternion INVERTED => invertedRotation;

    private void SetRotations()
    {
        horizontalRotation = Quaternion.Euler(-90f, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);

        invertedRotation = Quaternion.Inverse(transform.rotation);
    }
    #endregion

    private void Awake()
    {
        SetRotations();
    }


}
