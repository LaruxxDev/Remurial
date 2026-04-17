using UnityEngine;
using System.Collections;

public class PlayerConfiguration : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private GameObject player;

    [Header("Configuration")]

    #region Movement
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    public float MOVESPEED => moveSpeed;


    [SerializeField] float cameraModifier;
    [SerializeField] float cameraMoveSpeed;
    public float CAMERAMOVESPEED => cameraMoveSpeed;

    [Header("Delete?")]
    [SerializeField] float turnSpeed;
    public float TURNSPEED => turnSpeed;
    #endregion

    // Borrar?
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

    #region Camera
    [Header("Camera")]
    [SerializeField] float cameraCD;
    public float CAMERACD => cameraCD;

    [SerializeField] private float nextCameraTime = 0f;

    public bool CanUseCamera()
    {
        return Time.time >= nextCameraTime;
    }

    public void ResetCamera()
    {
        nextCameraTime = Time.time + cameraCD;
    }
    #endregion

    #region Aim    
    [Header("Aim")]
    [SerializeField] float sensitivity;
    public float SENSITIVITY => sensitivity;
    #endregion

    private void Awake()
    {
        // Borrar?
        SetRotations();

        cameraMoveSpeed = moveSpeed * (cameraModifier / 100);
    }



    // TEMPORAL
    public void LoadData(SavePointData spawnData, PlayerSaveData playerData)
    {
        // Position and Rotation
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.position = spawnData.position;
            rb.rotation = spawnData.rotation;
        }


        // health and other stats
    }
}

[System.Serializable]
public struct PlayerSaveData
{
    // hp and other stats
}