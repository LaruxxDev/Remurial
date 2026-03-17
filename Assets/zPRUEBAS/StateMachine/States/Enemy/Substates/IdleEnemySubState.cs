using UnityEngine;

public class IdleEnemySubState : EnemyState
{
    public override string Name => "Idle SubState";
    public IdleEnemySubState(StateMachine STATEMACHINE, EnemyGeneral ENEMY) : base(STATEMACHINE, ENEMY) { }


    public override void Enter()
    {
        base.Enter();


        Debug.Log("Entering: Idle");


        ENEMY.CONFIGURATION.ResetWander();

        //ENEMY.ANIMATION.SetAnimation(EnemyAnimation.Idle);
    }

    public override void Update()
    {
        // Wander
        if (ENEMY.CONFIGURATION.canWander)
        {
            STATEMACHINE.ChangeState(ENEMY.STATES.WanderSubState(STATEMACHINE));
        }
    }
}
