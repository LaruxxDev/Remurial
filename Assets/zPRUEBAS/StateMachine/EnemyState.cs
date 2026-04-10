using UnityEngine;

public abstract class EnemyState : State
{
    protected StateMachine STATEMACHINE;
    protected EnemyGeneral ENEMY;


    public EnemyState(EnemyGeneral ENEMY)
    {
        this.ENEMY = ENEMY;
    }

    public void SetMachine(StateMachine sm)
    {
        this.STATEMACHINE = sm;
    }

    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
}