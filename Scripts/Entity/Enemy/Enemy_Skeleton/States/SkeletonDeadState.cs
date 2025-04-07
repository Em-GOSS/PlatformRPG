using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonDeadState : EnemyState
{   
    private Enemy_Skeleton skeleton;
    public SkeletonDeadState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Skeleton _skeleton) : base(enemyBase, enemyStateMachine, animBoolName)
    {
        this.skeleton = _skeleton;
    }

    public override void AnimationFinishTrigger()
    {
        base.AnimationFinishTrigger();
    }

    public override void Enter()
    {
        base.Enter();
        skeleton.StopAllCoroutines();
        //Set last Ani
        skeleton.animator.SetBool(skeleton.lastAniBoolName, true);
        skeleton.animator.speed = 0;
        skeleton.bodyCollider.enabled = false;

        float dieKnockDir = Random.Range(-100,100) * 0.01f;

        skeleton.Rigidbody.velocity = new Vector2(dieKnockDir * 8, 12);

        skeleton.LifeOverAndDestoryBy(2f);
    }   

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }
}
