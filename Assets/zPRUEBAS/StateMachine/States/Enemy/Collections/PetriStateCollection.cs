using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriStateCollection : EnemyStateCollection
{
    private EnemyGeneral ENEMY;
    public PetriStateCollection(EnemyGeneral enemy) => this.ENEMY = enemy;


    public override  void Start(StateMachine sm)
    {
        // States
        UnawareState = new UnawareState(ENEMY);
        AwareState = new AwareState(ENEMY);
        PetrifiedState = new PetrifiedState(ENEMY);

        // Build
        UnawareState.SetMachine(sm);
        AwareState.SetMachine(sm);
        PetrifiedState.SetMachine(sm);


        // SubStates
        IdleSubState = new IdleEnemySubState(ENEMY);
        WanderSubState = new WanderSubState(ENEMY);
        ChaseSubState = new ChaseSubState(ENEMY);
        AttackSubState = new AttackSubState(ENEMY);
        DeadSubState = new DeadSubState(ENEMY);

        // Main State
        mainState = UnawareState;
    }
}
