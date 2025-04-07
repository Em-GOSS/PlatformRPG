using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonStunnedState : EnemyState
{    
    Enemy_Skeleton skeleton;

    public SkeletonStunnedState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName, Enemy_Skeleton _enemy) : base(enemyBase, enemyStateMachine, animBoolName)
    {
        skeleton = _enemy;
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = skeleton.stunned_Duartion;
        skeleton.entityFX.InvokeRepeating("RedBlink", 0, 0.1f);
    }

    public override void Exit()
    {
        base.Exit();
        skeleton.entityFX.Invoke("CancelColorChange", 0f);
    }

    public override void Update()
    {
        base.Update();

        if(stateTimer < 0 && skeleton.IsGroundDectected())
        {
            enemyStateMachine.ChangeState(skeleton.skeletonBattleState);
        }
    }

}
