using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonAttackState : EnemyState
{   
    Enemy_Skeleton skeleton;
    public SkeletonAttackState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName,Enemy_Skeleton _enemy) : base(enemyBase, enemyStateMachine, animBoolName)
    {
        skeleton=_enemy;
    }

    public override void Enter()
    {
        base.Enter();
        skeleton.isAttacking = true;
    }

    public override void Exit()
    {
        base.Exit();
        skeleton.isAttacking = false;
    }

    public override void Update()
    {
        base.Update();

        skeleton.SetZeroVelocity();

        if(triggerCalled)
        {
            enemyStateMachine.ChangeState(skeleton.skeletonBattleState);
        }
    }

}
