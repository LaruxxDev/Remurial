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
        // Configuración inicial
        // Idle
        if (ENEMY.CONFIGURATION.hasIdle)
            ENEMY.STATES.IdleSubState.SetMachine(lowLevelMachine);

        // Wander
        if (ENEMY.CONFIGURATION.hasWander)
            ENEMY.STATES.WanderSubState.SetMachine(lowLevelMachine);


        ENEMY.ANIMATION.SetBool("isChasing", false);

        lowLevelMachine.ChangeState(ENEMY.STATES.IdleSubState);
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        // Player Detection
        if (ENEMY.CONFIGURATION.hasAware)
        {
            // Aware State
            if (ENEMY.COLLISION.PLAYER)
            {
                STATEMACHINE.ChangeState(ENEMY.STATES.AwareState);
            }
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
        }

        base.Update();
    }

    public override void FixedUpdate()
    {
        lowLevelMachine.FixedUpdate();

        //ENEMY.ANIMATION.HandleAnimatorValues(ENEMY.Rigidbody, ENEMY.CONFIGURATION.WANDERSPEED);
        ENEMY.ANIMATION.HandleAnimatorValues(ENEMY.NAVMESH.velocity, ENEMY.Rigidbody.transform, ENEMY.CONFIGURATION.WANDERSPEED);

    }

    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
