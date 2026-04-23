using UnityEngine;

public abstract class PlayerState : State
{
    protected StateMachine StateMachine;
    protected PlayerGeneral Player;

    public PlayerState(StateMachine StateMachine, PlayerGeneral Player)
    {
        this.StateMachine = StateMachine;
        this.Player = Player;
    }

    public virtual void FixedUpdate() { }
    public virtual void LateUpdate() { }
}