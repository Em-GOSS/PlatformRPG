using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{     
    protected Direction direction=Direction.right;
    public Rigidbody2D Rigidbody{get; protected set;}
    public Animator animator{get; private set;}
    public EntityFX entityFX{get; private set;}
    public SpriteRenderer spriteRenderer{get; private set;}
    public CharacterStats stats {get; private set;}
    public CapsuleCollider2D bodyCollider {get; private set;}


    //Entity_Situation 
    public float FacingDirection{get=>direction==Direction.left?-1:1;} //use -1 or 1 to show the direction
    
    [Header("Entity_Properties")]
    // public Vector2 CurrentVelocity=new Vector2(); 

    [Space(10)][Header("Knockback Info")]
    [SerializeField] protected Vector2 knockbackStrength;
    [SerializeField] protected float knockbackDuartion;
    protected bool isKnockbacked;

    [Header("Stunned Info")]
    [SerializeField] protected float stunnedDuartion;
    [SerializeField] protected GameObject counterImage;
    public float stunned_Duartion{get => stunnedDuartion; }
    protected bool canBeStunned;
   

    [Space(10)][Header("Collision info")]
    [SerializeField] protected Transform attackCheck;
    public Transform attack_Check{get => attackCheck;}

    [SerializeField] protected float attackCheckRadius;
    public float attack_CheckRadius{get => attackCheckRadius;}

    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected float groundCheckDistance;

    [SerializeField] protected Transform wallCheck;
    [SerializeField] protected float wallCheckDistance;

    [SerializeField] protected LayerMask whatIsGround;
    [SerializeField] protected LayerMask whatIsWall;


    public bool isAttacking;
    public bool isDead {get; private set;}


    public System.Action onFlipped;

    public virtual void Awake() 
    {

    }

    public virtual void Start() 
    {   
        animator = this.GetComponentInChildren<Animator>();
        Rigidbody = this.GetComponent<Rigidbody2D>();
        entityFX = this.GetComponent<EntityFX>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        stats = this.GetComponent<CharacterStats>();
        bodyCollider = this.GetComponent<CapsuleCollider2D>();
    }

    protected virtual void Update() 
    {

    }

    public virtual void SlowEntityBy(float _slowPercentage, float slowDuartion)
    {
        animator.speed = animator.speed * (1 - _slowPercentage);
    }

    protected virtual void ReturnDefaultSpeed()
    {
        animator.speed = 1;
    }



    //Flip
    public void FlipDirection()
    {   
        if(isAttacking)
            return;
        direction = direction == Direction.left?Direction.right:Direction.left; 
        transform.Rotate(0,180,0);
        
        if(onFlipped != null)
            onFlipped();
    }

    public void FlipController(float _x) 
    {       

        if(_x > 0 && direction == Direction.left)
            FlipDirection();

        else if(_x < 0 && direction == Direction.right)
            FlipDirection();
    }

    //Velocity
    public void SetZeroVelocity()
    {   
        if(isKnockbacked)
            return;
        
        Rigidbody.velocity = new Vector2(0, 0);
    }

    public void SetVelocity(float xVelocity,float yVelocity)
    {   
        if(isKnockbacked)
            return;

        Rigidbody.velocity = new Vector2(xVelocity, yVelocity);
        FlipController(xVelocity);

    }

    public virtual void DamageImpact() => StartCoroutine(HitKnockback());       

    protected virtual IEnumerator HitKnockback() 
    {   
        isKnockbacked=true;
        
        Rigidbody.velocity = new Vector2(knockbackStrength.x * -FacingDirection, knockbackStrength.y);

        yield return new WaitForSeconds(knockbackDuartion);

        isKnockbacked=false;
    }   

    public virtual bool CanBeStunned()
    {
        if(canBeStunned)
        {
            CloseCounterAttackWindow();
            return true;
        }

        return false;
    }

    public virtual void OpenCounterAttackWindow() 
    {
        canBeStunned=true;
        counterImage.SetActive(true);
    }

    public virtual void CloseCounterAttackWindow() 
    {
        canBeStunned=false;
        counterImage.SetActive(false);
    }

    public virtual void Die()
    {   
        isDead = true;
    }

    //Collection Dectection
    public virtual bool IsGroundDectected()=>Physics2D.Raycast(groundCheck.position,Vector2.down,groundCheckDistance,whatIsGround);
    public virtual bool IsWallDectected()=>Physics2D.Raycast(wallCheck.position,Vector2.right,wallCheckDistance,whatIsWall);
    protected virtual void OnDrawGizmos() 
    {
        Gizmos.DrawLine(groundCheck.position,new Vector3(groundCheck.position.x,groundCheck.position.y-groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position,new Vector3(wallCheck.position.x+wallCheckDistance*FacingDirection,wallCheck.position.y));
        Gizmos.DrawWireSphere(attackCheck.position,attackCheckRadius);
    }

}
