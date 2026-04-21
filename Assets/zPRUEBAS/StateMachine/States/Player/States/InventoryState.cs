using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class InventoryState : PlayerState
{
    public override string Name => "Inventory State";
    public InventoryState(StateMachine STATEMACHINE, PlayerGeneral PLAYER) : base(STATEMACHINE, PLAYER) { }

    public override void Enter()
    {
        base.Enter();

        PLAYER.INPUTTRANSFORMER.ToggleInputMap("ui");

        // Pantalla congelada
        PLAYER.MOVEMENT.VelocityIdle();
        Time.timeScale = 0f;

        PLAYER.INVENTORY.OpenInventory();
    }

    public override void Update()
    {
        // Navegar
        Vector2 nav = PLAYER.INPUTTRANSFORMER.INPUTNAVEGATENORMAL;

        if (nav.x > 0.05f)
            PLAYER.INVENTORY.NavigateRight();
        else if (nav.x < -0.05f)
            PLAYER.INVENTORY.NavigateLeft();


        // Interactuar con el objeto
        if (PLAYER.INPUTTRANSFORMER.CONFIRM == 1f)
        {
            // Consumir Input
            PLAYER.INPUTTRANSFORMER.ProcessInputConfirm(0f);

            PLAYER.INVENTORY.TryActionCurrentItem(out bool wantsInspect, out GameObject inspectTarget);

            if (wantsInspect && inspectTarget != null)
            {
                PLAYER.inspectionItem = inspectTarget;
                STATEMACHINE.ChangeState(PLAYER.STATES.InspectState(STATEMACHINE));
            }
        }

        // Salir del inventario
        if (PLAYER.INPUTTRANSFORMER.ESCDOS == 1f)
        {
            // Consumir Input
            PLAYER.INPUTTRANSFORMER.ProcessInputEscDos(0f);

            STATEMACHINE.ChangeState(PLAYER.STATES.NeutralState(STATEMACHINE));
        }
    }

    public override void Exit()
    {
        base.Exit();

        PLAYER.INVENTORY.CloseInventory();

        Time.timeScale = 1f;
    }
}
