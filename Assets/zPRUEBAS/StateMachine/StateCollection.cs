using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateCollection
{
    private PlayerGeneral player;
    public StateCollection(PlayerGeneral player) => this.player = player;



    // States
    public NeutralState NeutralState(StateMachine sm) => new NeutralState(sm, player);
    public OnCameraState OnCameraState(StateMachine sm) => new OnCameraState(sm, player);
    public DialogueState DialogueState(StateMachine sm) => new DialogueState(sm, player);


    // SubStates
    public IdleSubState IdleSubState(StateMachine sm) => new IdleSubState(sm, player);
    public MoveSubState MoveSubState(StateMachine sm) => new MoveSubState(sm, player);
    public CameraIdleSubState CameraIdleSubState(StateMachine sm) => new CameraIdleSubState(sm, player);
    public CameraMoveSubState CameraMoveSubState(StateMachine sm) => new CameraMoveSubState(sm, player);
}
