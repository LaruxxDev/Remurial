using UnityEngine;

public class WanderSubState : EnemyState
{
    public override string Name => "Wander SubState";
    public WanderSubState(EnemyGeneral ENEMY) : base(ENEMY) { }


    public override void Enter()
    {
        base.Enter();

        Debug.Log("Entering: Wander");

        ENEMY.MOVEMENT.SetRandomDestination();

        //ENEMY.ANIMATION.SetAnimation(EnemyAnimation.Movement);
    }

    public override void Update()
    {
        // Idle
        if (ENEMY.MOVEMENT.HasArrived())
        {
            STATEMACHINE.ChangeState(ENEMY.STATES.idle);
        }
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
