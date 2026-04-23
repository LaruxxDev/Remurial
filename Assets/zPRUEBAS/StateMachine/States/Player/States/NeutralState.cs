using UnityEngine;

public class NeutralState : PlayerState
{
    private StateMachine _subMachine;
    public StateMachine subMachine => _subMachine;
    public override string Name => "Neutral State";

    public NeutralState(StateMachine stateMachine, PlayerGeneral player) 
        : base(stateMachine, player)
    {
        _subMachine = new StateMachine();
    }

    public override void Enter()
    {
        Player.Input.OnMoveEvent      += HandleMove;
        Player.Input.OnAimStarted  += HandleAimStart;
        Player.Input.OnFlashStarted += HandleFlash;

        _subMachine.ChangeState(Player.States.IdleSubState(_subMachine));
    }

    public override void Exit()
    {
        Player.Input.OnMoveEvent   -= HandleMove;
        Player.Input.OnAimStarted  -= HandleAimStart;
        Player.Input.OnFlashStarted -= HandleFlash;

        Player.Movement.SetMoveInput(Vector2.zero); 
    }

    public override void FixedUpdate()
    {
        _subMachine.FixedUpdate();
        Player.Movement.MoveThirdPerson(); 
    }
    public override void Update()
    {
        _subMachine.Update();
    }

    public override void LateUpdate()
    {
        _subMachine.LateUpdate();
    }

    // ── Handlers ────────────────────────────────────────────

    private void HandleMove(Vector2 dir)
    {
        Player.Movement.SetMoveInput(dir);
    }

    private void HandleAimStart()
    {
        CameraManager.SwitchCamera(Player.firstPersonCamera);
        StateMachine.ChangeState(Player.States.OnCameraState(StateMachine));
    }

    private void HandleFlash()
    {
        Player.Movement.Flash();
    }
}