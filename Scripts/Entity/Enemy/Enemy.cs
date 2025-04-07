using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : Entity
{   
    [Header("Enemy_Properties")]        
    [Space(2)][Header("Move Info")] 
    public float moveSpeed;
    public float normalMoveSpeed;
    public float idleTime;


    [Space(2)][Header("Mask Info")]
    [SerializeField] protected LayerMask whatIsPlyaer;

    [Space(2)][Header("Attack_Battle Info")] 
    [SerializeField] protected GameObject NoticeImage;
    [SerializeField] protected float NoticeEffectStrength;
    [SerializeField] protected float effectDuartion;
    
    [HideInInspector] public float enemyAttackLastTime;
    [SerializeField] public float maxChaseDistance;
    [SerializeField] public float minHartedDistance;
    [SerializeField] public float battleTime;
    [SerializeField] private float enemyAttackCoolTime;
    [SerializeField] public float battleMoveSpeed;
    [SerializeField] protected Transform battleCheck;
    [SerializeField] protected float noticeDistance;
    [SerializeField] protected float attackDistance;
    public float attack_Distance{get=>attackDistance;}

    public EnemyStateMachine stateMachine;
    public string lastAniBoolName{get; private set;}
    private bool isTimeFreeze = false;  
      
    public override void Awake()
    {   
        base.Awake();
        stateMachine=new EnemyStateMachine();
    }

    protected override void Update() 
    {   
        base.Update();
        stateMachine.currentEnemyState.Update();
        // Debug.Log(IsPlayerDectected().collider.gameObject.name+"I See!");
    }

    public override void SlowEntityBy(float _slowPercentage, float slowDuartion)
    {
        base.SlowEntityBy(_slowPercentage, slowDuartion);
        moveSpeed *= (1 - _slowPercentage);

        Invoke("ReturnDefaultSpeed", slowDuartion);
    }

    protected override void ReturnDefaultSpeed()
    {
        base.ReturnDefaultSpeed();

        moveSpeed = normalMoveSpeed;
    }

    public virtual void AssignLastAniName(string _animBoolName)
    {
        lastAniBoolName = _animBoolName;
    }

    public void FreezeTime(bool _isTimeFreeze) 
    {   
        isTimeFreeze = _isTimeFreeze;

        if(isTimeFreeze)
        {   
            animator.speed = 0;
            moveSpeed = 0;
        }
        else
        {
            animator.speed = 1;
            moveSpeed = normalMoveSpeed;
        }
        
    }

    public void FreezeTimeFor(float freezeDuartion)
    {
        StartCoroutine(FreezeTimeCoroutine(freezeDuartion));
    }
    protected virtual IEnumerator FreezeTimeCoroutine(float freezeDuartion)
    {
        FreezeTime(true);

        yield return new WaitForSeconds(freezeDuartion);

        FreezeTime(false);
    }

    public bool canEnemyAttack()
    {
        if(Time.time-enemyAttackLastTime>=enemyAttackCoolTime)
        {
            return true;
        }

        return false;
    }

    public IEnumerator TransformNoticeWindow()
    {
        NoticeImage.SetActive(true);
        float currentScaleX = NoticeImage.transform.localScale.x;
        while(!Mathf.Approximately(currentScaleX,0.8f))
        {   
            currentScaleX=Mathf.Clamp(currentScaleX+NoticeEffectStrength*Time.deltaTime,0.6f,0.8f);
            NoticeImage.transform.localScale = currentScaleX*Vector3.one;
            yield return null;
        }
        
        while(!Mathf.Approximately(currentScaleX,0.6f))
        {   
            currentScaleX=Mathf.Clamp(currentScaleX-NoticeEffectStrength*Time.deltaTime,0.6f,0.8f);
            NoticeImage.transform.localScale = currentScaleX*Vector3.one;
            yield return null;
        }
        yield return new WaitForSeconds(effectDuartion);
        NoticeImage.SetActive(false);
    }


    public void AnimationTrigger()=>stateMachine.currentEnemyState.AnimationFinishTrigger();


    public bool IsTimeFreeze()
    {
        return isTimeFreeze;
    }

    
    //Collision Dectection
    public virtual RaycastHit2D IsPlayerDectected()=>Physics2D.Raycast(wallCheck.position,Vector2.right*FacingDirection,noticeDistance,whatIsPlyaer);

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.DrawLine(battleCheck.position,battleCheck.position+Vector3.right*FacingDirection*noticeDistance);
        Gizmos.color=Color.red;
        Gizmos.DrawLine(battleCheck.position,battleCheck.position+Vector3.right*FacingDirection*attackDistance);
    }
    

    public void LifeOverAndDestoryBy(float _second)
    {
        Destroy(this.gameObject, _second);
    }
    
}
