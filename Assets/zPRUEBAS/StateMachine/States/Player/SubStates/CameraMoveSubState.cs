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
        if (PLAYER.INPUTTRANSFORMER.INPUTNAVIGATENORMAL.magnitude < 0.1f)
            STATEMACHINE.ChangeState(PLAYER.STATES.CameraIdleSubState(STATEMACHINE));
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        PLAYER.MOVEMENT.VelocityCamera(PLAYER.INPUTTRANSFORMER.INPUTNAVIGATENORMAL, PLAYER.mainCamera);
    }
}
