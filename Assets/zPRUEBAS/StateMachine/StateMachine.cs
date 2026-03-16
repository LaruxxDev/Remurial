using UnityEngine;

public class StateMachine
{
    public State state { get; private set; }

    public void ChangeState(State newState)
    {
        state?.Exit();
        state = newState;
        state?.Enter();
    }

    public void Update() => state?.Update();

    public void FixedUpdate()
    {
        if (state is PlayerState ps) ps.FixedUpdate();
    }

    public void LateUpdate()
    {
        if (state is PlayerState ps) ps.LateUpdate();
    }
}