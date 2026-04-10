using UnityEngine;

public class MoveSubState : PlayerState
{
    public override string Name => "Walking SubState";
    public MoveSubState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }


    public override void Enter()
    {
        base.Enter();

        //PLAYER.ANIMATION.SetAnimation(PlayerAnimation.Movement);
    }

    public override void Update()
    {
        // Idle
        if (PLAYER.INPUTTRANSFORMER.INPUTVECTORNORMAL.magnitude < 0.1f)
            STATEMACHINE.ChangeState(PLAYER.STATES.IdleSubState(STATEMACHINE));
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        PLAYER.MOVEMENT.VelocityMovement(PLAYER.INPUTTRANSFORMER.INPUTVECTORNORMAL, PLAYER.mainCamera);
    }
}
