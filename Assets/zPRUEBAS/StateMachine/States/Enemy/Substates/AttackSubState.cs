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


    public float timer = 1f;
    public override void Update()
    {
        // Enter Chase after attack animation delay
        timer -= Time.deltaTime;

        switch (timer)
        {
            case <= 0f:
                
                STATEMACHINE.ChangeState(ENEMY.STATES.ChaseSubState(STATEMACHINE));            
                break;

            case <= 0.5f:

                // Stop
                ENEMY.MOVEMENT.ChasePlayer(ENEMY.Rigidbody.position);
                break;
        }

            
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
