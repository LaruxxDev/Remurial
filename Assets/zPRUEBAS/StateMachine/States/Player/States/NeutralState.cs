using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class NeutralState : PlayerState
{
    private StateMachine lowLevelMachine;
    public StateMachine subMachine => lowLevelMachine;

    public override string Name => "Neutral State";
    public NeutralState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER)
    {
        lowLevelMachine = new StateMachine();
    }

    public override void Enter()
    {
        lowLevelMachine.ChangeState(PLAYER.STATES.IdleSubState(lowLevelMachine));
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        base.Update();


        // Revelar foto
        if (PLAYER.INPUTTRANSFORMER.INPUTINTERACT == 1f && PLAYER.heldPhoto != null)
        {
            PLAYER.heldPhoto.RevelarInstantaneo();
        }


        // Camera
        if (PLAYER.INPUTTRANSFORMER.INPUTCAMERA == 1f)
        {
            CameraManager.SwitchCamera(PLAYER.firstPersonCamera);
            STATEMACHINE.ChangeState(PLAYER.STATES.OnCameraState(STATEMACHINE));
        }


        // Flash
        if (PLAYER.INPUTTRANSFORMER.INPUTFLASH > 0f)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputFlash(0f);

            PLAYER.MOVEMENT.Flash();
        }
    }
    
    public override void FixedUpdate() => lowLevelMachine.FixedUpdate();
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
