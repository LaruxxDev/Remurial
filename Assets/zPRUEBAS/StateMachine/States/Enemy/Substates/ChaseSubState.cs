using UnityEngine;

public class ChaseSubState : EnemyState
{
    public override string Name => "Chase SubState";
    public ChaseSubState(EnemyGeneral ENEMY) : base(ENEMY) { }


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
        if (ENEMY.COLLISION.ATTACK)
            STATEMACHINE.ChangeState(ENEMY.STATES.attack);

    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
