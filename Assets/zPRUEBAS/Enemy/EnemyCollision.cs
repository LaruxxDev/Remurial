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
    [SerializeField] LayerMask obstacleLayer;
    [SerializeField] float detectionRadius;
    [SerializeField] bool requiresRange;
    [SerializeField] bool requiresLineOfSight;
    [SerializeField] bool detectionGizmoz;


    public Transform detectedPlayer;


    [Header("Attack")]
    [SerializeField] float attackRadius;
    [SerializeField] bool attackGizmoz;



    [Header("Flash")]
    public bool FLASH;

    public bool PHOTOMADE;

    public bool REVEALED;


    public bool GROUND => Physics.Raycast(groundCheck.position, -transform.up, -groundDistance, groundLayer);

    public bool PLAYER
    {
        get
        {
            if (!requiresRange && detectedPlayer != null)
                return true;

            bool seen = CheckLineOfSight(detectionRadius, out Transform found);

            detectedPlayer = found;
            return seen;
        }
    }

    public bool ATTACK
    {
        get
        {
            bool seen = CheckLineOfSight(attackRadius, out Transform found);

            return seen;
        }
    }



    private bool CheckLineOfSight(float radius, out Transform found)
    {
        Collider[] hits = Physics.OverlapSphere(playerCheck.position, radius, playerLayer);

        foreach (Collider hit in hits)
        {
            if (!requiresLineOfSight)
            {
                found = hit.transform;
                return true;
            }

            Vector3 directionToPlayer = hit.bounds.center - playerCheck.position;
            float distanceToPlayer = directionToPlayer.magnitude;

            if (!Physics.Raycast(playerCheck.position, directionToPlayer.normalized, distanceToPlayer, obstacleLayer))
            {
                found = hit.transform;
                return true;
            }
        }

        found = null; 
        return false;
    }


    private void OnDrawGizmos()
    {
        if (groundGizmoz)
        {
            if (GROUND) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;

            Gizmos.DrawRay(groundCheck.position, -transform.up * -groundDistance);
        }

        if (detectionGizmoz)
        {
            if (PLAYER) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(playerCheck.position, detectionRadius);
        }

        if (attackGizmoz)
        {
            if (ATTACK) Gizmos.color = Color.green;
            else Gizmos.color = Color.red;

            Gizmos.DrawWireSphere(playerCheck.position, attackRadius);
        }
    }

}
