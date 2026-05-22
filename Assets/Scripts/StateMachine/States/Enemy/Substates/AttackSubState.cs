using UnityEngine;

public class AttackSubState : EnemyState
{
    public override string Name => "Attack SubState";
    public AttackSubState(EnemyGeneral ENEMY) : base(ENEMY) { }


    private float timer;


    public override void Enter()
    {
        base.Enter();

        timer = ENEMY.CONFIGURATION.ATTACKCD;

        // Dañar al Jugador
        if (ENEMY.COLLISION.detectedPlayer != null)
        {
            PlayerGeneral player = ENEMY.COLLISION.detectedPlayer.parent.GetComponentInChildren<PlayerGeneral>();

            if (player != null)
                player.HEALTH.TakeDamage(ENEMY.CONFIGURATION.DAMAGE);
            
        }

        ENEMY.MOVEMENT.StopMovement();
        ENEMY.ANIMATION.SetTrigger("Attack");
    }


    public override void Update()
    {
        timer -= Time.deltaTime;

        // CD terminado
        if (timer <= 0f)
        {
            if (ENEMY.COLLISION.ATTACK)
            {
                STATEMACHINE.ChangeState(ENEMY.STATES.AttackSubState);
            }
            else if (ENEMY.CONFIGURATION.hasChase)
            {
                STATEMACHINE.ChangeState(ENEMY.STATES.ChaseSubState);
            }
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
