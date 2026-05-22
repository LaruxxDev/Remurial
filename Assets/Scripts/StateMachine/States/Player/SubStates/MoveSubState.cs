using UnityEngine;

public class MoveSubState : PlayerState
{
    public override string Name => "Walking SubState";
    public MoveSubState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }


    public override void Enter()
    {
        base.Enter();

        //PLAYER.ANIMATION.SetAnimation(AnimatorManager.Movement);
        //PLAYER.ANIMATION.HandleAnimatorValues(0f, 0f);

        //PLAYER.ANIMATION.PlayAnimation("Walk");
    }

    public override void Update()
    {
        // Idle
        if (PLAYER.INPUTTRANSFORMER.INPUTMOVEMENTNORMAL.magnitude < 0.1f)
            STATEMACHINE.ChangeState(PLAYER.STATES.IdleSubState(STATEMACHINE));
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        PLAYER.MOVEMENT.VelocityMovement(PLAYER.INPUTTRANSFORMER.INPUTMOVEMENTNORMAL, PLAYER.mainCamera);
    }
}
