using UnityEngine;
using System.Collections.Generic;
public class FlashEnemy : MonoBehaviour
{
    [Header("Ajustes del Flash")]
    public LayerMask obstacleLayer;

    [Header("List")]
    [SerializeField] private List<EnemyCollision> enemiesFlashed = new List<EnemyCollision>();


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
                enemyCollision.FLASH = true;

                if (!enemiesFlashed.Contains(enemyCollision))
                    enemiesFlashed.Add(enemyCollision);
            }
        }
        else
        {
            Debug.Log("Bloqueado por: " + hit.collider.gameObject.name);
        }   
    }


    private void OnDisable()
    {
        // Desmarcar el flash en el enemigo
        foreach (EnemyCollision enemy in enemiesFlashed)
        {
            enemy.FLASH = false;
        }

        enemiesFlashed.Clear();
    }
}