using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PetriStateCollection : EnemyStateCollection
{
    private EnemyGeneral ENEMY;
    public PetriStateCollection(EnemyGeneral enemy) => this.ENEMY = enemy;


    public override void Start(StateMachine sm)
    {
        // States
        //unaware = new UnawareState(sm, enemy);
        //aware = new AwareState(sm, enemy);

        // SubStates
        //idle = new IdleEnemySubState(sm, enemy);
        //wander = new WanderSubState(sm, enemy);
        //chase = new ChaseSubState(sm, enemy);
        //attack = new AttackSubState(sm, enemy);

        // Main State
        mainState = unaware;
    }
}
