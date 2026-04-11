using UnityEngine;

public class CameraIdleSubState : PlayerState
{
    public override string Name => "Camera Idle SubState";

    public CameraIdleSubState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }

    public override void Enter()
    {
        base.Enter();

        PLAYER.MOVEMENT.VelocityIdle();
        //PLAYER.ANIMATION.SetAnimation(PlayerAnimation.Idle);
    }

    public override void Update()
    {
        if (PLAYER.INPUTTRANSFORMER.INPUTVECTORNORMAL.magnitude > 0.1f)
        {
            STATEMACHINE.ChangeState(PLAYER.STATES.CameraMoveSubState(STATEMACHINE));
        }
    }
}
