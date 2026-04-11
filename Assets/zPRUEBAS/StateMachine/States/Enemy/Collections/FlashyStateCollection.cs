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
        AwareState = new AwareState(ENEMY);

        // Build
        AwareState.SetMachine(sm);


        // SubStates
        ChaseSubState = new ChaseSubState(ENEMY);
        AttackSubState = new AttackSubState(ENEMY);

        // Main State
        mainState = AwareState;
    }
}
