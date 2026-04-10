using UnityEngine;

public abstract class PlayerState : State
{
    protected StateMachine STATEMACHINE;
    protected PlayerGeneral PLAYER;

    public PlayerState(StateMachine STATEMACHINE, PlayerGeneral PLAYER)
    {
        this.STATEMACHINE = STATEMACHINE;
        this.PLAYER = PLAYER;
    }

    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
}