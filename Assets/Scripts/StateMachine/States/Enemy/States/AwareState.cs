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
        // Configuraci�n inicial
        // Chase
        if (ENEMY.CONFIGURATION.hasChase)
            ENEMY.STATES.ChaseSubState.SetMachine(lowLevelMachine);

        // Attack
        if (ENEMY.CONFIGURATION.hasAttack)
            ENEMY.STATES.AttackSubState.SetMachine(lowLevelMachine);

        lowLevelMachine.ChangeState(ENEMY.STATES.ChaseSubState);
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        // Losing Detection
        if (!ENEMY.COLLISION.PLAYER)
        {
            // Unaware State
            if (ENEMY.CONFIGURATION.hasUnaware)
                STATEMACHINE.ChangeState(ENEMY.STATES.UnawareState);
        }


        // Camera Detection
        if (ENEMY.COLLISION.PHOTOMADE)
        {

        }

        // Revealed
        if (ENEMY.COLLISION.REVEALED)
        {
            if (ENEMY.enemyType == EnemyGeneral.EnemyType.birbEnemy)
            {
                // DEAD
                Debug.Log("Dead");
                Object.Destroy(ENEMY.Rigidbody.gameObject);
            }
        }

        // Flash Detection
        if (ENEMY.COLLISION.FLASH)
        {
            // Petrified State
            if (ENEMY.CONFIGURATION.hasPetrified)
                STATEMACHINE.ChangeState(ENEMY.STATES.PetrifiedState);

            // Flashy Enemy
            if (ENEMY.enemyType == EnemyGeneral.EnemyType.flashyEnemy)
            {
                ENEMY.CONFIGURATION.AllowFlashMovement();
            }
        }


        base.Update();
    }
    
    public override void FixedUpdate() => lowLevelMachine.FixedUpdate();
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
