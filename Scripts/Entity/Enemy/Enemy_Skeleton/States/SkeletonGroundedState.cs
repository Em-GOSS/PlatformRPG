using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonGroundedState : EnemyState
{   
    protected Enemy_Skeleton skeleton;

    private Transform playerTransform;

    public SkeletonGroundedState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Skeleton _enemy) : base(enemyBase, enemyStateMachine, animBoolName)
    {
        skeleton=_enemy;
    }

    public override void Enter()
    {
        base.Enter();
        playerTransform = PlayerManager.Instance.player.transform;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if(skeleton.IsPlayerDectected() || Vector2.Distance(playerTransform.position,skeleton.transform.position) < skeleton.minHartedDistance)
        {
            enemyStateMachine.ChangeState(skeleton.skeletonBattleState);
        }   

    }

}
