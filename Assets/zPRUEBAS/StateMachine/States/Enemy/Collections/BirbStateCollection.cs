using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BirbStateCollection : EnemyStateCollection
{
    private EnemyGeneral ENEMY;
    public BirbStateCollection(EnemyGeneral enemy) => this.ENEMY = enemy;


    public override void Start(StateMachine sm)
    {
        // States
        unaware = new UnawareState(ENEMY);
        aware = new AwareState(ENEMY);

        // Build
        unaware.SetMachine(sm);
        aware.SetMachine(sm);


        // SubStates
        idle = new IdleEnemySubState(ENEMY);
        wander = new WanderSubState(ENEMY);
        chase = new ChaseSubState(ENEMY);
        attack = new AttackSubState(ENEMY);

        // Main State
        mainState = unaware;
    }
}
