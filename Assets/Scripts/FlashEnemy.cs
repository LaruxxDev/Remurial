using UnityEngine;
using System.Collections.Generic;

public class FlashEnemy2 : MonoBehaviour
{
    [SerializeField] private List<EnemyCollision> enemiesFlashed = new List<EnemyCollision>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            // Marcar el flash en el enemigo
            EnemyCollision enemyCollision = other.GetComponent<EnemyCollision>();

            if (enemyCollision != null)
            {
                enemyCollision.FLASH = true;

                enemiesFlashed.Add(enemyCollision);         
            }
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
