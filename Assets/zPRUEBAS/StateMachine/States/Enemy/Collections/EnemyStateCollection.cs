using UnityEngine;

public abstract class EnemyStateCollection
{
    public State mainState;

    public abstract void Start(StateMachine sm);


    // States
    #region Enemy1
    public UnawareState unaware;
    public AwareState aware;

    public IdleEnemySubState idle;
    public WanderSubState wander;
    public ChaseSubState chase;
    public AttackSubState attack;
    #endregion

    #region Enemy2
    //public UnawareState unaware;
    //public AwareState aware;

    //public IdleEnemySubState idle;
    //public WanderSubState wander;
    //public ChaseSubState chase;
    //public AttackSubState attack;
    #endregion

    #region Enemy3
    //public UnawareState unaware;
    //public AwareState aware;

    //public IdleEnemySubState idle;
    //public WanderSubState wander;
    //public ChaseSubState chase;
    //public AttackSubState attack;
    #endregion
}