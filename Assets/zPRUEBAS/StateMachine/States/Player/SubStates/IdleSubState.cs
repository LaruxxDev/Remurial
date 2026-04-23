using UnityEngine;

public class IdleSubState : PlayerState
{
    public override string Name => "Idle SubState";

    public IdleSubState(StateMachine stateMachine, PlayerGeneral player) 
        : base(stateMachine, player) { }

    public override void Enter()
    {
        Player.Movement.StopMovement();
        // Player.Animator.SetAnimation(PlayerAnimation.Idle);
    }

    public override void Update()
    {
        if (Player.Movement.MoveInput.magnitude > 0.1f)
            StateMachine.ChangeState(Player.States.MoveSubState(StateMachine));
    }

}