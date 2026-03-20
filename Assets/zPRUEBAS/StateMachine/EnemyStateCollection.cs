using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateCollection
{
    private EnemyGeneral enemy;
    public EnemyStateCollection(EnemyGeneral enemy) => this.enemy = enemy;



    // States
    public UnawareState UnawareState(StateMachine sm) => new UnawareState(sm, enemy);
    public AwareState AwareState(StateMachine sm) => new AwareState(sm, enemy);


    // SubStates
    public IdleEnemySubState IdleEnemySubState(StateMachine sm) => new IdleEnemySubState(sm, enemy);
    public WanderSubState WanderSubState(StateMachine sm) => new WanderSubState(sm, enemy);
    public ChaseSubState ChaseSubState(StateMachine sm) => new ChaseSubState(sm, enemy);
    public AttackSubState AttackSubState(StateMachine sm) => new AttackSubState(sm, enemy);
}
