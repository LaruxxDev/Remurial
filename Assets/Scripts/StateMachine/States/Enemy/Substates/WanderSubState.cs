using UnityEngine;

public class WanderSubState : EnemyState
{
    public override string Name => "Wander SubState";
    public WanderSubState(EnemyGeneral ENEMY) : base(ENEMY) { }


    public override void Enter()
    {
        base.Enter();

        ENEMY.MOVEMENT.SetRandomDestination();

        //ENEMY.ANIMATION.SetAnimation(EnemyAnimation.Movement);
    }

    public override void Update()
    {
        // Idle
        if (ENEMY.MOVEMENT.HasArrived())
        {
            if (ENEMY.CONFIGURATION.hasIdle)
            STATEMACHINE.ChangeState(ENEMY.STATES.IdleSubState);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
