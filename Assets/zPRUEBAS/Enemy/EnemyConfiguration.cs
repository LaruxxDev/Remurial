using Unity.VisualScripting;
using UnityEngine;
using System.Collections;

public class EnemyConfiguration : MonoBehaviour
{
    [Header("Configuration")]

    #region Stats
    [Header("Stats")]
    [SerializeField] private int damage;
    public int DAMAGE => damage;
    #endregion

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

    public bool canMove;



    public void ResetWander()
    {
        StartCoroutine(ResetWanderRoutine(waitTime));
    }

    private IEnumerator ResetWanderRoutine(float delay)
    {
        canMove = false;

        yield return new WaitForSeconds(delay);

        canMove = true;
    }
    #endregion

    #region Follow
    [Header("Follow")]
    [SerializeField] float baseFollowSpeed;
    [SerializeField] float followSpeed;
    public float FOLLOWSPEED => followSpeed;

    [SerializeField] float turnSpeed;
    public float TURNSPEED => turnSpeed;


    public void AllowFlashMovement()
    {
        StartCoroutine(FlashMovementRoutine(waitTime));
    }

    private IEnumerator FlashMovementRoutine(float delay)
    {
        followSpeed = baseFollowSpeed;

        yield return new WaitForSeconds(delay);

        followSpeed = 0f;
    }
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

    #region States
    [Header("States")]
    public bool hasAware;
    public bool hasUnaware;
    public bool hasPetrified;

    [Header("SubStates")]
    public bool hasIdle;
    public bool hasWander;
    public bool hasChase;
    public bool hasAttack;
    public bool hasDead;
    #endregion


    private void Awake()
    {
        SetRotations();

        // Flash Enemy empieza sin moverse
        if (canMove)
            followSpeed = baseFollowSpeed;
    }
}
