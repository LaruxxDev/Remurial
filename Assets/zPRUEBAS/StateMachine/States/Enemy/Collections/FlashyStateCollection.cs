using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashyStateCollection : EnemyStateCollection
{
    private EnemyGeneral ENEMY;
    public FlashyStateCollection(EnemyGeneral enemy) => this.ENEMY = enemy;


    public override void Start(StateMachine sm)
    {
        //// States
        //unaware = new UnawareState(sm, ENEMY);
        //aware = new AwareState(sm, ENEMY);

        //// SubStates
        //idle = new IdleEnemySubState(sm, ENEMY);
        //wander = new WanderSubState(sm, ENEMY);
        //chase = new ChaseSubState(sm, ENEMY);
        //attack = new AttackSubState(sm, ENEMY);

        //// Main State
        //mainState = unaware;
    }
}
