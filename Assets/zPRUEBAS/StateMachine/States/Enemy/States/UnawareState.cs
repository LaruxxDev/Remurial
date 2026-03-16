using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class UnawareState : EnemyState
{
    private StateMachine lowLevelMachine;
    public StateMachine subMachine => lowLevelMachine;

    public override string Name => "Unaware State";
    public UnawareState(StateMachine STATEMACHINE, EnemyGeneral ENEMY) : base(STATEMACHINE, ENEMY)
    {
        lowLevelMachine = new StateMachine();
    }

    public override void Enter()
    {
        lowLevelMachine.ChangeState(ENEMY.STATES.IdleEnemySubState(lowLevelMachine));
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        // Detection logic

        base.Update();
    }
    
    public override void FixedUpdate() => lowLevelMachine.FixedUpdate();
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
