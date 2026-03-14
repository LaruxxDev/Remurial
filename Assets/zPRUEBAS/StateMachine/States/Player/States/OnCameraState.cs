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
        //lowLevelMachine.ChangeState(PLAYER.STATES.IdleSubState(lowLevelMachine));
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        base.Update();
    }
    
    public override void FixedUpdate() => lowLevelMachine.FixedUpdate();
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
