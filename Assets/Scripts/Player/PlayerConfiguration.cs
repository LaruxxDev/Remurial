using UnityEngine;
using System.Collections;

public class PlayerConfiguration : MonoBehaviour
{
    [Header("Player Reference")]
    [SerializeField] private GameObject player;

    [Header("Configuration")]
    [Header("Flash")]
    [SerializeField] public Light flashLight;          // Luz del flash
    [SerializeField] public float flashDuration;       // Duracion del flash en segundos
    [SerializeField] public float flashMaxIntensity;   // Intensidad maxima de la luz durante el flash
    [SerializeField] public GameObject flashArea;      // Area del flash 

    #region Stats
    [Header("Stats")]

    [Header("Vida")]
    public int health = 6;
    public int maxHealth = 6;
    public int healthRegen = 1;
    #endregion

    #region Movement
    [Header("Movement")]
    [SerializeField] float moveSpeed;
    public float MOVESPEED => moveSpeed;


    [SerializeField] [Range(0f,100f)] float cameraModifier;
    [SerializeField] float cameraMoveSpeed;
    public float CAMERAMOVESPEED => cameraMoveSpeed;


    [SerializeField] float turnSpeed;
    public float TURNSPEED => turnSpeed;
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
        // Convierte CameraModifier en un porcentaje
        cameraMoveSpeed = moveSpeed * (cameraModifier / 100);
    }



    #region Save & Load
    public void SaveData(ref PlayerSaveData data)
    {
        // HP
        data.health = health;
    }

    public void LoadData(SavePointData spawnData, PlayerSaveData playerData)
    {
        // Position and Rotation
        Rigidbody rb = player.GetComponent<Rigidbody>();

        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            rb.position = spawnData.position;

            Quaternion rot = spawnData.rotation;
            rb.rotation = (rot == default) ? Quaternion.identity : rot.normalized;
        }

        // HP
        health = playerData.health;
    }
    #endregion
}

[System.Serializable]
public struct PlayerSaveData
{
    // HP
    public int health;
}