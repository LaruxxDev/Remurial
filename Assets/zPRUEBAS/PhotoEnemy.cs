using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhotoEnemy : MonoBehaviour
{
    [Header("Ajustes de Foto")]
    public LayerMask obstacleLayer;

    [Header("List")]
    [SerializeField] private List<EnemyCollision> enemiesCaught = new List<EnemyCollision>();


    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Enemy"))
            return;

        // 1. Calculamos la dirección y la distancia exacta hacia el enemigo
        Vector3 directionToEnemy = other.bounds.center - transform.position;
        float distanceToEnemy = directionToEnemy.magnitude;

        bool blocked = Physics.Raycast(transform.position, directionToEnemy, out RaycastHit hit, distanceToEnemy, obstacleLayer);

        // 2. Tiramos el Raycast
        if (!blocked)
        {
            EnemyCollision enemyCollision = other.GetComponent<EnemyCollision>();

            // Marcar como flasheado
            if (enemyCollision != null)
            {
                enemyCollision.PHOTOMADE = true;

                if (!enemiesCaught.Contains(enemyCollision))
                    enemiesCaught.Add(enemyCollision);
            }
        }
        else
        {
            Debug.Log("Bloqueado por: " + hit.collider.gameObject.name);
        }
    }

    public void ClearList()
    {
        enemiesCaught.Clear();
    }
}
