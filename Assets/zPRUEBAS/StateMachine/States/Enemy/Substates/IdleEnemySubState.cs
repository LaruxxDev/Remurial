using UnityEngine;

public class IdleEnemySubState : EnemyState
{
    public override string Name => "Idle SubState";
    public IdleEnemySubState(EnemyGeneral ENEMY) : base(ENEMY) { }


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
        if (ENEMY.CONFIGURATION.hasWander && ENEMY.CONFIGURATION.canWander)
            STATEMACHINE.ChangeState(ENEMY.STATES.WanderSubState);
    }
}
