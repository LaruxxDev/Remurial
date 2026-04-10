using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashyStateCollection : EnemyStateCollection
{
    private EnemyGeneral ENEMY;
    public FlashyStateCollection(EnemyGeneral enemy) => this.ENEMY = enemy;


    public override void Start(StateMachine sm)
    {
        // States
        UnawareState = new UnawareState(ENEMY);
        AwareState = new AwareState(ENEMY);

        // Build
        UnawareState.SetMachine(sm);
        AwareState.SetMachine(sm);


        // SubStates
        IdleSubState = new IdleEnemySubState(ENEMY);
        WanderSubState = new WanderSubState(ENEMY);
        ChaseSubState = new ChaseSubState(ENEMY);
        AttackSubState = new AttackSubState(ENEMY);

        // Main State
        mainState = UnawareState;
    }
}
