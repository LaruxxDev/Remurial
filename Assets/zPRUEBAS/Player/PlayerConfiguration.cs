using UnityEngine;
using System.Collections;

public class PlayerConfiguration : MonoBehaviour
{
    [Header("Configuration")]

    #region Movement
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    public float MOVESPEED => moveSpeed;

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
