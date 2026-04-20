using UnityEngine;

public class AttackSubState : EnemyState
{
    public override string Name => "Attack SubState";
    public AttackSubState(EnemyGeneral ENEMY) : base(ENEMY) { }


    public override void Enter()
    {
        base.Enter();

        // Dañar al Jugador
        if (ENEMY.COLLISION.detectedPlayer != null)
        {
            PlayerGeneral player = ENEMY.COLLISION.detectedPlayer.parent.GetComponentInChildren<PlayerGeneral>();
            player.HEALTH.TakeDamage(ENEMY.CONFIGURATION.DAMAGE);
        }


        //ENEMY.ANIMATION.SetAnimation(EnemyAnimation.Attack);
    }


    public float timer = 1f;
    public override void Update()
    {
        // Enter Chase after attack animation delay
        timer -= Time.deltaTime;

        switch (timer)
        {
            case <= 0f:
                if (ENEMY.CONFIGURATION.hasChase)
                    STATEMACHINE.ChangeState(ENEMY.STATES.ChaseSubState);            
                break;

            case <= 0.5f:

                // Stop
                ENEMY.MOVEMENT.StopMovement();
                break;
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
