using UnityEngine;

public class AttackSubState : EnemyState
{
    public override string Name => "Attack SubState";
    public AttackSubState(StateMachine STATEMACHINE, EnemyGeneral ENEMY) : base(STATEMACHINE, ENEMY) { }


    public override void Enter()
    {
        base.Enter();


        Debug.Log("Entering: Attack");

        //ENEMY.ANIMATION.SetAnimation(EnemyAnimation.Attack);
    }

    public override void Update()
    {

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
