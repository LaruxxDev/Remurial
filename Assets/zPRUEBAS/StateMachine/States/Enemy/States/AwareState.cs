using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class AwareState : EnemyState
{
    private StateMachine lowLevelMachine;
    public StateMachine subMachine => lowLevelMachine;

    public override string Name => "Aware State";
    public AwareState(StateMachine STATEMACHINE, EnemyGeneral ENEMY) : base(STATEMACHINE, ENEMY)
    {
        lowLevelMachine = new StateMachine();
    }

    public override void Enter()
    {
        lowLevelMachine.ChangeState(ENEMY.STATES.ChaseSubState(lowLevelMachine));
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        // Losing detection, going back to unawareness
        if (!ENEMY.COLLISION.PLAYER)
        {
            STATEMACHINE.ChangeState(ENEMY.STATES.UnawareState(STATEMACHINE));
        }

        base.Update();
    }
    
    public override void FixedUpdate() => lowLevelMachine.FixedUpdate();
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
