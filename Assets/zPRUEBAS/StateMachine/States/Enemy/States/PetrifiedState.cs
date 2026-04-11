using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class PetrifiedState : EnemyState
{
    private StateMachine lowLevelMachine;
    public StateMachine subMachine => lowLevelMachine;

    public override string Name => "Petrified State";
    public PetrifiedState(EnemyGeneral ENEMY) : base(ENEMY)
    {
        lowLevelMachine = new StateMachine();
    }

    public override void Enter()
    {
        // Configuración inicial
        // Dead
        if (ENEMY.CONFIGURATION.hasDead)
            ENEMY.STATES.DeadSubState.SetMachine(lowLevelMachine);


        lowLevelMachine.ChangeState(ENEMY.STATES.DeadSubState);
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        base.Update();
    }
    
    public override void FixedUpdate() => lowLevelMachine.FixedUpdate();
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
