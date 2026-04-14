using UnityEngine;
using System.Collections.Generic;
public class FlashEnemy : MonoBehaviour
{
    [Header("Ajustes del Flash")]
    public LayerMask obstacleLayer;
    public LayerMask enemyLayer;
    private BoxCollider _boxCollider;
    public void Start()
    {
        Debug.Log("FLASH ENEMY START");
        _boxCollider = GetComponent<BoxCollider>(); 
    }

    public void Update()
    {
        Vector3 center = _boxCollider.bounds.center;
        Vector3 halfExtents = _boxCollider.bounds.extents;
        Quaternion rotation = transform.rotation;

        Collider[] hitEnemies = Physics.OverlapBox(center, halfExtents, rotation, enemyLayer);
        foreach (Collider enemy in hitEnemies)        
        {
            Vector3 directionToEnemy = enemy.bounds.center - transform.position;
            float distanceToEnemy = directionToEnemy.magnitude;

            // 2. Tiramos el Raycast
            if (!Physics.Raycast(transform.position, directionToEnemy, distanceToEnemy, obstacleLayer))
            {
                Debug.Log("¡Línea de visión despejada! FLASH aplicado.");

                EnemyCollision enemyCollision = enemy.GetComponent<EnemyCollision>();

                if (enemyCollision != null)
                {
                    enemyCollision.FLASH = true;
                }
            }
            else
            {
                Debug.Log("El enemigo está en el área, pero algo bloquea la luz del flash.");
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log("Enemigo detectado en la zona del flash.");

            // 1. Calculamos la dirección y la distancia exacta hacia el enemigo
            Vector3 directionToEnemy = other.bounds.center - transform.position;
            float distanceToEnemy = directionToEnemy.magnitude;

            // 2. Tiramos el Raycast
            if (!Physics.Raycast(transform.position, directionToEnemy, distanceToEnemy, obstacleLayer))
            {
                Debug.Log("¡Línea de visión despejada! FLASH aplicado.");

                EnemyCollision enemyCollision = other.GetComponent<EnemyCollision>();

                if (enemyCollision != null)
                {
                    enemyCollision.FLASH = true;
                }
            }
            else
            {
                Debug.Log("El enemigo está en el área, pero algo bloquea la luz del flash.");
            }
        }
    }

}