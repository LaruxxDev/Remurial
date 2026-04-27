using UnityEngine;

public class NeutralState : PlayerState
{
    private StateMachine lowLevelMachine;
    public StateMachine subMachine => lowLevelMachine;

    public override string Name => "Neutral State";
    public NeutralState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER)
    {
        lowLevelMachine = new StateMachine();
    }

    public override void Enter()
    {
        lowLevelMachine.ChangeState(PLAYER.STATES.IdleSubState(lowLevelMachine));
    }

    public override void Update()
    {
        lowLevelMachine.Update();

        base.Update();


        // Revelar foto
        if (PLAYER.INPUTTRANSFORMER.LEFTCLICK == 1f && PLAYER.heldPhoto != null)
        {
            PLAYER.heldPhoto.RevelarInstantaneo();
        }


        // Camera
        if (PLAYER.INPUTTRANSFORMER.RIGHTCLICK == 1f)
        {
            STATEMACHINE.ChangeState(PLAYER.STATES.OnCameraState(STATEMACHINE));
        }


        // Flash
        if (PLAYER.INPUTTRANSFORMER.F == 1f)
        {
            // Consumir el input
            PLAYER.INPUTTRANSFORMER.ProcessInputF(0f);

            PLAYER.MOVEMENT.Flash();
        }


        // Inventory
        if (PLAYER.INPUTTRANSFORMER.TAB == 1f)
        {
            // Consumir el input
            PLAYER.INPUTTRANSFORMER.ProcessInputTab(0f);

            STATEMACHINE.ChangeState(PLAYER.STATES.InventoryState(STATEMACHINE));

            // Pruebas
            //PLAYER.HEALTH.TakeDamage(1);
            //PLAYER.savePointA.OnInteract();
        }


        // Bestiary
        if (PLAYER.INPUTTRANSFORMER.B == 1f)
        {
            // Consumir el input
            PLAYER.INPUTTRANSFORMER.ProcessInputB(0f);

            // Pruebas
            //PLAYER.HEALTH.RegenerateHealth(2);
            PLAYER.savePointB.OnInteract();
        }


        // Interact
        if (PLAYER.INPUTTRANSFORMER.E == 1f)
        {
            // Consumir el input
            PLAYER.INPUTTRANSFORMER.ProcessInputE(0f);

            if (PLAYER.COLLISION.INTERACT)
            {
                PLAYER.COLLISION.currentInteractable.Interact(PLAYER.COLLISION.interactableItem);

                STATEMACHINE.ChangeState(PLAYER.STATES.InspectState(STATEMACHINE));
            }
        }
    }
    
    public override void FixedUpdate() => lowLevelMachine.FixedUpdate();
    public override void LateUpdate() => lowLevelMachine.LateUpdate();
}
