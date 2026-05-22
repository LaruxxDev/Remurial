using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class DeadState : PlayerState
{
    public override string Name => "Dead State";
    public DeadState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }

    public override void Enter()
    {
        base.Enter();

        PLAYER.INPUTTRANSFORMER.ToggleInputMap("ui");

        // Pantalla congelada
        PLAYER.MOVEMENT.VelocityIdle();
        Time.timeScale = 0f;

        //PLAYER.ANIMATION.PlayAnimation("Dead");
    }

    public override void Update()
    {

    }
}
