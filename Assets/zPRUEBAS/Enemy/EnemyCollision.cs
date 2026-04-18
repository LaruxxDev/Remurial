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
    [SerializeField] bool detectionGizmoz;

    public Transform detectedPlayer;


    [Header("Attack")]
    [SerializeField] float attackRadius;
    [SerializeField] bool attackGizmoz;



    [Header("Flash")]
    public bool FLASH;

    public bool FOTOMADE;
    public bool REVELADO;

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


    public bool ATTACK
    {
        get
        {
            Collider[] hits = Physics.OverlapSphere(playerCheck.position, attackRadius, playerLayer);

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
