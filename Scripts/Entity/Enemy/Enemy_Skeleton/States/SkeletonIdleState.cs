using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonIdleState : SkeletonGroundedState
{
    public SkeletonIdleState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Skeleton _enemy) : base(enemyBase, enemyStateMachine, animBoolName, _enemy)
    {
        
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer=skeleton.idleTime;
        AudioManager.Instance.PlaySFX(19, 32, skeleton.gameObject.transform);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0f) 
        {
            enemyStateMachine.ChangeState(skeleton.skeletonMoveState);
        }

    }
}
