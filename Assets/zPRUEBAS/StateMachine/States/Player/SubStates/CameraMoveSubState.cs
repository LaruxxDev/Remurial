using UnityEngine;

public class CameraMoveSubState : PlayerState
{
    public override string Name => "Camera Move SubState";
    public CameraMoveSubState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }


    public override void Enter()
    {
        base.Enter();

        //PLAYER.ANIMATION.SetAnimation(PlayerAnimation.Movement);
    }

    public override void Update()
    {
        // Idle
        if (Player.Movement.MoveInput.magnitude < 0.1f)
            StateMachine.ChangeState(Player.States.CameraIdleSubState(StateMachine));
    }

}
