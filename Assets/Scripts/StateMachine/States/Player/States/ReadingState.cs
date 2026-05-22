using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class ReadingState : PlayerState
{
    public override string Name => "Reading State";
    public ReadingState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }

    public override void Enter()
    {
        base.Enter();

        // Pantalla congelada
        PLAYER.MOVEMENT.VelocityIdle();
        Time.timeScale = 0f;
    }

    public override void Update()
    {
        // Salir
        if (PLAYER.INPUTTRANSFORMER.E == 1f)
        {
            // Consumir el input
            PLAYER.INPUTTRANSFORMER.ProcessInputE(0f);

            STATEMACHINE.ChangeState(PLAYER.STATES.NeutralState(STATEMACHINE));
        }
    }

    public override void Exit()
    {
        base.Exit();

        Time.timeScale = 1f;
        PoemReadable.poema.SetActive(false);
    }
}
