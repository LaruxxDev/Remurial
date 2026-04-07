using UnityEngine;

public class FollowSubState : EnemyState
{
    public override string Name => "Follow SubState";
    public FollowSubState(StateMachine STATEMACHINE, EnemyGeneral ENEMY) : base(STATEMACHINE, ENEMY) { }


    public override void Enter()
    {
        base.Enter();


        Debug.Log("Entering: Follow");


        ENEMY.MOVEMENT.SetSpeed("follow");
        //ENEMY.ANIMATION.SetAnimation(EnemyAnimation.Movement);
    }

    public override void Update()
    {
        // Unaware
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
