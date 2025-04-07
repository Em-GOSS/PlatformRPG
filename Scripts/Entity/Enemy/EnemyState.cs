using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState 
{
    protected EnemyStateMachine enemyStateMachine;
    protected Enemy enemyBase;
    protected float stateTimer;
    protected bool triggerCalled;

    private string animBoolName;

    
    public EnemyState(Enemy enemyBase, EnemyStateMachine enemyStateMachine, string animBoolName )
    {
        this.enemyStateMachine=enemyStateMachine;
        this.enemyBase=enemyBase;
        this.animBoolName=animBoolName;
    }

    public virtual void Enter() 
    {
        enemyBase.animator.SetBool(animBoolName,true);
        triggerCalled=false;
    }

    public virtual void Exit() 
    {
        enemyBase.animator.SetBool(animBoolName,false);
        enemyBase.AssignLastAniName(animBoolName);
    }


    public virtual void Update() 
    {
        stateTimer-=Time.deltaTime;
    }

    public virtual void FixedUpdate() 
    {

    }

    public virtual void AnimationFinishTrigger() 
    {
        triggerCalled=true;
    }
}
