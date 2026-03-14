using UnityEngine;

public class WanderSubState : EnemyState
{
    public override string Name => "Wander SubState";
    public WanderSubState(StateMachine STATEMACHINE, EnemyGeneral ENEMY) : base(STATEMACHINE, ENEMY) { }


    public override void Enter()
    {
        base.Enter();

        //ENEMY.ANIMATION.SetAnimation(EnemyAnimation.Movement);
    }

    public override void Update()
    {
        // Idle
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        // Moving logic
    }
}
