using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class OnCameraState : PlayerState
{
    private StateMachine lowLevelMachine;
    public StateMachine subMachine => lowLevelMachine;

    public override string Name => "OnCamera State";
    public OnCameraState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER)
    {
        lowLevelMachine = new StateMachine();
    }

    public override void Enter()
    {
        lowLevelMachine.ChangeState(PLAYER.STATES.CameraIdleSubState(lowLevelMachine));
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        base.Update();

        PLAYER.PHOTO.CustomUpdate();

        // Neutral State
        if (PLAYER.INPUTTRANSFORMER.INPUTCAMERA == 0f)
        {
            CameraManager.SwitchCamera(PLAYER.thirdPersonCamera);
            STATEMACHINE.ChangeState(PLAYER.STATES.NeutralState(STATEMACHINE));
        }

        // Hacer foto
        if (PLAYER.INPUTTRANSFORMER.INPUTINTERACT > 0f)
        {
            // Consumir el input
            PLAYER.INPUTTRANSFORMER.ProcessInputInteract(0f);

            PLAYER.PHOTO.TakePhoto();
        }

        // Toggle Flash
        if (PLAYER.INPUTTRANSFORMER.INPUTFLASH > 0f)
        {
            // Consumir el input
            PLAYER.INPUTTRANSFORMER.ProcessInputFlash(0f);

            PLAYER.PHOTO.ToggleFlash();
        }
    }
    
    public override void FixedUpdate() => lowLevelMachine.FixedUpdate();
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
