using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class UnawareState : EnemyState
{
    private StateMachine lowLevelMachine;
    public StateMachine subMachine => lowLevelMachine;

    public override string Name => "Unaware State";
    public UnawareState(EnemyGeneral ENEMY) : base(ENEMY)
    {
        lowLevelMachine = new StateMachine();
    }

    public override void Enter()
    {
        ENEMY.STATES.idle.SetMachine(lowLevelMachine);
        ENEMY.STATES.wander.SetMachine(lowLevelMachine);

        lowLevelMachine.ChangeState(ENEMY.STATES.idle);
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        // Aware State
        if (ENEMY.COLLISION.PLAYER)
        {
            STATEMACHINE.ChangeState(ENEMY.STATES.aware);
        }

        base.Update();
    }
    
    public override void FixedUpdate() => lowLevelMachine.FixedUpdate();
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
