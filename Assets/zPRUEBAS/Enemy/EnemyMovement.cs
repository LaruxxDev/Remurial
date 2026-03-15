using UnityEngine;
using UnityEngine.AI;

public class EnemyMovement
{
    Rigidbody rigidbody;
    NavMeshAgent agent;
    EnemyConfiguration EnemyConfiguration;

    public EnemyMovement(Rigidbody rigidbody, NavMeshAgent agent, EnemyConfiguration EnemyConfiguration)
    {
        this.rigidbody = rigidbody;
        this.agent = agent;
        this.EnemyConfiguration = EnemyConfiguration;
    }

    #region Movement
    // Modifica la velocidad del NavMeshAgent según la acción que efectúa el enemigo
    public void SetSpeed(string action)
    {
        switch (action)
        {
            case "wander":
                agent.speed = EnemyConfiguration.WANDERSPEED;
                break;

            case "follow":
                agent.speed = EnemyConfiguration.FOLLOWSPEED;
                break;

            case "attack":
                agent.speed = 0;
                break;

            default:
                Debug.LogError("WRONG ACTION SELECTED");
                break;
        }      
    }


    public bool HasArrived()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }

    public void SetRandomDestination()
    {
        agent.SetDestination(GetRandomPoint());
    }


    public Vector3 GetRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * EnemyConfiguration.WANDERRADIUS;
        randomDirection.y = 0f;

        Vector3 randomPoint = rigidbody.transform.position + randomDirection;

        NavMeshHit hit;
        Vector3 finalPosition = rigidbody.transform.position;

        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, 1))
        {
            finalPosition = hit.position;
        }

        return finalPosition;
    }
    #endregion
}
