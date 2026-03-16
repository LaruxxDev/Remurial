using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollision : MonoBehaviour
{
    [Header("Ground")]
    [SerializeField] Transform groundCheck;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float groundDistance;
    [SerializeField] bool groundGizmoz;

    [Header("Detection")]
    [SerializeField] Transform playerCheck;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float detectionRadius;
    [SerializeField] bool playerGizmoz;

    public bool GROUND => Physics.Raycast(groundCheck.position, -transform.up, -groundDistance, groundLayer);
    public bool PLAYER => Physics.CheckSphere(playerCheck.position, detectionRadius, playerLayer);

    private void OnDrawGizmos()
    {
        if (groundGizmoz)
        {
            if (GROUND) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;

            Gizmos.DrawRay(groundCheck.position, -transform.up * -groundDistance);
        }

        if (playerGizmoz)
        {
            if (PLAYER) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(playerCheck.position, detectionRadius);
        }
    }

    private void Update()
    {
        if (PLAYER) Debug.Log("Player Detected");
    }
}
