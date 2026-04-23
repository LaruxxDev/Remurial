using UnityEngine;

public class OnCameraState : PlayerState
{
    private StateMachine _subMachine;
    public StateMachine subMachine => _subMachine;
    public override string Name => "OnCamera State";

    public OnCameraState(StateMachine stateMachine, PlayerGeneral player) 
        : base(stateMachine, player)
    {
        _subMachine = new StateMachine();
    }

    public override void Enter()
    {
        Player.Input.OnMoveEvent   += HandleMove;
        Player.Input.OnAimCanceled += HandleAimEnd;

        _subMachine.ChangeState(Player.States.CameraIdleSubState(_subMachine));
    }

    public override void Exit()
    {
        Player.Input.OnMoveEvent   -= HandleMove;
        Player.Input.OnAimCanceled -= HandleAimEnd;

        Player.Movement.SetMoveInput(Vector2.zero);
    }

    public override void Update()      => _subMachine.Update();
    public override void FixedUpdate()
    {
        _subMachine.FixedUpdate();
        Player.Movement.MoveFirstPerson();
    }
    public override void LateUpdate()  => _subMachine.LateUpdate();

    // ── Handlers ────────────────────────────────────────────

    private void HandleMove(Vector2 dir)
    {
        Player.Movement.SetMoveInput(dir);
    }

    private void HandleAimEnd()
    {
        CameraManager.SwitchCamera(Player.thirdPersonCamera);
        StateMachine.ChangeState(Player.States.NeutralState(StateMachine));
    }
}