using UnityEngine;

public class FlashEnemy : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
             Debug.Log("FLASH");
            EnemyCollision enemyCollision = other.GetComponent<EnemyCollision>();
            if (enemyCollision != null)
            {
                enemyCollision.FLASH = true;
            }
        }

    }
}
