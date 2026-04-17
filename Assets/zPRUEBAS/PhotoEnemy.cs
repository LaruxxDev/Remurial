using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PhotoEnemy : MonoBehaviour
{
    public List<EnemyCollision> enemiesCaught = new List<EnemyCollision>();


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            EnemyCollision enemyCollision = other.GetComponent<EnemyCollision>();

            if (enemyCollision != null)
            {
                enemyCollision.PHOTOMADE = true;
                
                if (!enemiesCaught.Contains(enemyCollision))
                    enemiesCaught.Add(enemyCollision);
            }
        }
    }

    public void ClearList()
    {
        enemiesCaught.Clear();
    }
}
