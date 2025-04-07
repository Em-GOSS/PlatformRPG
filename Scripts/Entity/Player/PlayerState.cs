using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState 
{
    protected PlayerStateMachine playerStateMachine;
    protected Player player;
    protected float xInput;
    protected float yInput;

    protected float stateTimer;
    protected bool triggerCalled;

    private string animBoolName;
    

    public PlayerState(Player _player,PlayerStateMachine _playerStateMachine,string _animBoolName)
    {
        this.player=_player;
        this.playerStateMachine=_playerStateMachine;
        this.animBoolName=_animBoolName;
    }

    public virtual void Enter() 
    {
       player.animator.SetBool(animBoolName,true);
       triggerCalled=false;
    }

    public virtual void Exit()
    {
        player.animator.SetBool(animBoolName,false);
    }

    public virtual void Update()
    {       
        stateTimer-=Time.deltaTime;

        xInput=Input.GetAxisRaw("Horizontal");
        yInput=Input.GetAxisRaw("Vertical");
        player.animator.SetFloat("yVelocity",player.Rigidbody.velocity.y);
    }

    public virtual void FixedUpdate() 
    {
        
    }

    public virtual void AnimationFinishTrigger() 
    {
        triggerCalled=true;
    }
}
