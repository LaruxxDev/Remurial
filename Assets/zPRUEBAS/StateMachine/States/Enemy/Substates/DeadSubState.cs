using UnityEngine;

public class DeadSubState : EnemyState
{
    public override string Name => "Dead SubState";
    public DeadSubState(EnemyGeneral ENEMY) : base(ENEMY) { }


    public override void Enter()
    {
        base.Enter();

        Debug.Log("Entering: Dead");

        //ENEMY.ANIMATION.SetAnimation(EnemyAnimation.Dead);
    }

    public override void Update()
    {
        base.Update();

        ENEMY.MOVEMENT.StopMovement();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }
}
