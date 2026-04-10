using UnityEngine;

public abstract class EnemyStateCollection
{
    public State mainState;

    public abstract void Start(StateMachine sm);


    [Header("States")]
    public UnawareState UnawareState;
    public AwareState AwareState;
    public PetrifiedState PetrifiedState;

    [Header("SubStates")]
    public IdleEnemySubState IdleSubState;
    public WanderSubState WanderSubState;
    public ChaseSubState ChaseSubState;
    public AttackSubState AttackSubState;
    public DeadSubState DeadSubState;
}