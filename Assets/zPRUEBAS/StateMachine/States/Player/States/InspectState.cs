using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class InspectState : PlayerState
{
    public override string Name => "Inspect State";
    public InspectState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }

    public override void Enter()
    {
        base.Enter();

        // Pantalla congelada
        PLAYER.MOVEMENT.VelocityIdle();
        Time.timeScale = 0f;

        PLAYER.INSPECT.EnterInspectionMode(PLAYER.inspectionItem);
    }

    public override void Update()
    {
        PLAYER.INSPECT.CustomUpdate();

        if (PLAYER.INPUTTRANSFORMER.F == 1f)
        {  
            // Consumir el input
            PLAYER.INPUTTRANSFORMER.ProcessInputF(0f);

            STATEMACHINE.ChangeState(PLAYER.STATES.NeutralState(STATEMACHINE));
        }
    }

    public override void Exit()
    {
        if (PLAYER.INSPECT.isInspecting)
        {
            PLAYER.INSPECT.ExitInspectionMode();
        }
    }
}
