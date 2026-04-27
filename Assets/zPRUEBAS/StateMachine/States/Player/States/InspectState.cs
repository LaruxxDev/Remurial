using UnityEngine;

public class InspectState : PlayerState
{
    public override string Name => "Inspect State";
    public InspectState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }

    public override void Enter()
    {
        base.Enter();

        PLAYER.INPUTTRANSFORMER.ToggleInputMap("ui");

        // Pantalla congelada
        PLAYER.MOVEMENT.VelocityIdle();
        Time.timeScale = 0f;

        CameraManager.SwitchCamera(PLAYER.inspectionCamera, CameraManager.BlendStyle.Instant);
        
        PLAYER.INSPECT.EnterInspectionMode(PLAYER.inspectionItem);
    }

    public override void Update()
    {
        PLAYER.INSPECT.CustomUpdate();


        // Volver a Neutral
        if (PLAYER.INPUTTRANSFORMER.ESCDOS == 1f)
        {  
            // Consumir el input
            PLAYER.INPUTTRANSFORMER.ProcessInputEscDos(0f);

            STATEMACHINE.ChangeState(PLAYER.STATES.NeutralState(STATEMACHINE));
        }
    }

    public override void Exit()
    {
        if (PLAYER.INSPECT.isInspecting)
        {
            PLAYER.INSPECT.ExitInspectionMode();
        }

        Time.timeScale = 1f;

        PLAYER.INPUTTRANSFORMER.ProcessInputMovement(Vector2.zero);
        PLAYER.INPUTTRANSFORMER.ProcessInputAim(Vector2.zero);

        PLAYER.INPUTTRANSFORMER.ToggleInputMap("player");

        CameraManager.SwitchCamera(PLAYER.thirdPersonCamera, CameraManager.BlendStyle.Instant);
    }
}
