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


    #region Wander
    // Setea la velocidad y el destino aleatorio
    public void SetRandomDestination()
    {
        agent.speed = EnemyConfiguration.WANDERSPEED;

        agent.SetDestination(GetRandomPoint());
    }

    // Genera un punto aleatorio dentro del área
    public Vector3 GetRandomPoint()
    {
        // Genera un punto aleatorio
        Vector3 randomDirection = Random.insideUnitSphere * EnemyConfiguration.WANDERRADIUS;
        randomDirection.y = 0f;

        Vector3 randomPoint = rigidbody.transform.position + randomDirection;

        NavMeshHit hit;
        Vector3 finalPosition = rigidbody.transform.position;

        // Designa el punto si es correcto
        if (NavMesh.SamplePosition(randomPoint, out hit, 2f, 1))       
            finalPosition = hit.position;

        return finalPosition;
    }

    // Detecta si ha llegado a su objetivo
    public bool HasArrived()
    {
        return agent.remainingDistance <= agent.stoppingDistance;
    }
    #endregion

    #region Follow
    // Setea la velocidad y el jugador como el destino
    public void ChasePlayer(Vector3 playerPosition)
    {
        agent.speed = EnemyConfiguration.FOLLOWSPEED;

        agent.SetDestination(playerPosition);
    }
    #endregion
}
