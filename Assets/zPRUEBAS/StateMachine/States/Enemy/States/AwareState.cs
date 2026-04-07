using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class AwareState : EnemyState
{
    private StateMachine lowLevelMachine;
    public StateMachine subMachine => lowLevelMachine;

    public override string Name => "Aware State";
    public AwareState(EnemyGeneral ENEMY) : base(ENEMY)
    {
        lowLevelMachine = new StateMachine();
    }

    public override void Enter()
    {
        ENEMY.STATES.chase.SetMachine(lowLevelMachine);
        ENEMY.STATES.attack.SetMachine(lowLevelMachine);

        lowLevelMachine.ChangeState(ENEMY.STATES.chase);
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        // Losing detection, going back to unawareness
        if (!ENEMY.COLLISION.PLAYER)
        {
            STATEMACHINE.ChangeState(ENEMY.STATES.unaware);
        }

        base.Update();
    }
    
    public override void FixedUpdate() => lowLevelMachine.FixedUpdate();
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
