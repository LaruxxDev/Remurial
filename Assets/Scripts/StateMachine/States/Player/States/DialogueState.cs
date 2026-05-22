using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class DialogueState : PlayerState
{
    public override string Name => "Dialogue State";
    public DialogueState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }

    public override void Enter()
    {
        base.Enter();

        // Pantalla congelada
        PLAYER.MOVEMENT.VelocityIdle();
        Time.timeScale = 0f;

        DialogueManager.Instance.OnDialogueEnd += ExitDialogue;
    }

    public override void Update()
    {
        // Acanzar / Skip
        if (PLAYER.INPUTTRANSFORMER.LEFTCLICK == 1f)
        {
            PLAYER.INPUTTRANSFORMER.ProcessInputLeftClick(0f);

            DialogueManager.Instance.HandleInput();
        }
    }

    public override void Exit()
    {
        base.Exit();

        Time.timeScale = 1f;

        // Activar Inputs
        PLAYER.INPUTTRANSFORMER.EnableInputs(true);

        DialogueManager.Instance.OnDialogueEnd -= ExitDialogue;
    }


    // De vuelta a Neutral
    private void ExitDialogue()
    {
        STATEMACHINE.ChangeState(PLAYER.STATES.NeutralState(STATEMACHINE));
    }
}