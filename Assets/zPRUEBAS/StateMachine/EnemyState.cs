using UnityEngine;

public abstract class EnemyState : State
{
    protected StateMachine STATEMACHINE;
    protected EnemyGeneral ENEMY;

    public EnemyState(StateMachine STATEMACHINE, EnemyGeneral ENEMY)
    {
        this.STATEMACHINE = STATEMACHINE;
        this.ENEMY = ENEMY;
    }

    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
}