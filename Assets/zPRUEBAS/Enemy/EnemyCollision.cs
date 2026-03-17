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

    public Transform detectedPlayer;

    public bool GROUND => Physics.Raycast(groundCheck.position, -transform.up, -groundDistance, groundLayer);

    public bool PLAYER
    {
        get
        {
            Collider[] hits = Physics.OverlapSphere(playerCheck.position, detectionRadius, playerLayer);

            if (hits.Length > 0)
            {
                detectedPlayer = hits[0].transform;
                return true;
            }

            return false;
        }
    }


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

}
