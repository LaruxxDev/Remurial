using UnityEngine;

public class ChaseSubState : EnemyState
{
    public override string Name => "Chase SubState";
    public ChaseSubState(StateMachine STATEMACHINE, EnemyGeneral ENEMY) : base(STATEMACHINE, ENEMY) { }


    public override void Enter()
    {
        base.Enter();


        Debug.Log("Entering: Chase");

        

        //ENEMY.ANIMATION.SetAnimation(EnemyAnimation.Movement);
    }

    public override void Update()
    {
        // Keep chasing
        if (!ENEMY.COLLISION.ATTACK)
            ENEMY.MOVEMENT.ChasePlayer(ENEMY.COLLISION.detectedPlayer.position);

        // Attack
        //if (ENEMY.COLLISION.ATTACK)     
            //STATEMACHINE.ChangeState(ENEMY.STATES.AttackSubState(STATEMACHINE));
        
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
