using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sword_Skill_Controller : MonoBehaviour
{       
    private Sword_Type sword_Type;

    private Player player;

    private Animator animator;
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rigidbody_2D;
    private CircleCollider2D circleCollider2D;

    [Header("Sword Return")]
    [SerializeField] private float swordReturnSpeed;  

    [Header("Sword Bouncy")]
    [SerializeField] private bool canSwordBouncy; 
    [SerializeField] private int max_BouncyAmount;
    [SerializeField] private float bouncyRangeRadius;
    [SerializeField] private float swordBouncySpeed;
    private List<Transform> targets = new List<Transform>();
    private int currentTargetIndex;

    [Header("Sword Pierce")]
    [SerializeField] private int max_PierceAmount;
    private float piercedAmount = 0;

    [Header("Sword Spin")]
    [SerializeField] private float maxTravelDistance;
    [SerializeField] private float spinDuartion;
    [SerializeField] private float spinHitCoolTime;
    private float spinTimer;
    private float spinHitTimer;
    private bool wasStopped;
    private bool isSpinning;
    public bool canStopSpinning{get => !wasStopped && !isStuckIntoGround && sword_Type == Sword_Type.Spin;}

    [Header("Booleans")]
    private bool canRotate = true;
    private bool isReturning = false;
    private bool isBouncying = false;

    private bool isStuckIntoGround = false;
    // private bool isStuckIntoEnemy= false;

    
    [Header("Disappear Details")]
    [SerializeField] private float destroy_Distance;
    [SerializeField] private float destroy_Duration;
    
    private void Awake() 
    {   
        animator = this.GetComponentInChildren<Animator>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();
        rigidbody_2D = this.GetComponent<Rigidbody2D>();
        circleCollider2D = this.GetComponent<CircleCollider2D>();
    }

    public void SetUpSword(Sword_Type sword_Type, Vector2 launchSpeed, float sword_Gravity, Player player)
    {       
        this.player = player;
        this.sword_Type = sword_Type;
        rigidbody_2D.velocity = launchSpeed;
        rigidbody_2D.gravityScale = sword_Gravity;

        if(sword_Type == Sword_Type.Spin)
        {
            isSpinning = true;
        }

        Invoke("DestroySword", destroy_Duration);
    }

    public void ReturnSword() 
    {   
        animator.SetBool("Rotation",true);
        rigidbody_2D.isKinematic = false;
        rigidbody_2D.constraints = RigidbodyConstraints2D.None;

        transform.parent = null;

        canRotate = true;
        isReturning = true;

        // isStuckIntoEnemy = false;
        isStuckIntoGround = false;
    }


    private void Update()
    {   
        if(Vector2.Distance(transform.position, player.transform.position) >= destroy_Distance)
        {
            DestroySword();
        }

        SwordRotate();
        SwordReturning();
        SwordBouncy();
        SwordSpin();
    }

    private void SwordRotate()
    {
        if (canRotate)
            transform.right = rigidbody_2D.velocity;
    }

    private void SwordReturning()
    {
        if (isReturning)
        {   
            transform.parent = null;
            rigidbody_2D.velocity = (player.transform.position - transform.position).normalized * swordReturnSpeed;
            if (Vector2.Distance(player.transform.position, transform.position) < 1)
            {   
                player.CatchSword();
            }
        }
    }

    private void SwordBouncy()
    {
        if (isBouncying)
        {    
            if(targets[currentTargetIndex] == null)
            {   
                isBouncying = false;
                return;
            }
            rigidbody_2D.velocity = (targets[currentTargetIndex].transform.position - transform.position).normalized * swordBouncySpeed;
            if (Vector2.Distance(targets[currentTargetIndex].transform.position, transform.position) < 0.4f)
            {   
                SwordSkillDamage(targets[currentTargetIndex].gameObject.GetComponent<Enemy>());
                currentTargetIndex++;
                max_BouncyAmount--;
                if(currentTargetIndex >= targets.Count)
                {
                    if(max_BouncyAmount <= 0)
                    {
                        canSwordBouncy = false;
                        isBouncying = false;
                        rigidbody_2D.velocity = new Vector2(0,0);
                        transform.position = targets[currentTargetIndex-1].transform.position;
                    }                  
                    currentTargetIndex = 0;
                }   
                    
            }
        }
    }

    private void SwordSpin() 
    {
        if(isSpinning)
        {   
            if(!wasStopped)
            {
                if(Vector2.Distance(transform.position, player.transform.position) >= maxTravelDistance)
                {   
                    Spinning();
                }
            }
            else
            {
                spinTimer -= Time.deltaTime;

                if(spinTimer < 0)
                {
                    isSpinning = false;
                    isReturning = true;
                    rigidbody_2D.constraints = RigidbodyConstraints2D.None;
                }

                spinHitTimer -= Time.deltaTime;
                if(spinHitTimer < 0) 
                {   
                    spinHitTimer = spinHitCoolTime;
                    Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position,1f);
                    foreach(Collider2D hit in hits)
                    {   
                        Enemy hit_Enemy = hit.GetComponent<Enemy>();
                        if(hit_Enemy!=null) 
                        {
                            SwordSkillDamage(hit_Enemy);
                        }

                    }
                } 
            }

            
        }
    }

    public void Spinning()
    {   
        
        animator.SetBool("Rotation",true);
        wasStopped = true;
        rigidbody_2D.constraints = RigidbodyConstraints2D.FreezeAll;
        spinTimer = spinDuartion;
             
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if(isReturning)
            return;

        if(canSwordBouncy && sword_Type == Sword_Type.Bounce)
        {
            isBouncying = SearchTargets(collider);
            if(isBouncying)
            {   
                animator.SetBool("Rotation", true);
                return;
            }
        }

        else if(sword_Type == Sword_Type.Pierce)
        {
            if(collider.GetComponent<Enemy>() != null)
                piercedAmount++;
        }

        Enemy enemy = collider.GetComponent<Enemy>(); 
        if(enemy != null)
        {
            SwordSkillDamage(enemy);
        }


        StuckInto(collider);
    }

    private bool SearchTargets(Collider2D collider)
    {       

        if (collider.GetComponent<Enemy>() != null)
        {
            if (targets.Count == 0)
            {
                Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, bouncyRangeRadius, LayerMask.GetMask("Enemy"));

                foreach (Collider2D hit in hits)
                {
                    targets.Add(hit.transform);
                }
            }
        }
        if(targets.Count >= 1)
            return true;
        else
            return false;
    }

    private void StuckInto(Collider2D collider)
    {   
        if(sword_Type == Sword_Type.Pierce && piercedAmount <= max_PierceAmount)
            return;
        if(isSpinning && collider.GetComponent<Enemy>() != null)
        {
            Spinning();
            return; 
        }
        
        canRotate = false;
        isSpinning = false;

        circleCollider2D.enabled = false;

        rigidbody_2D.isKinematic = true;

        rigidbody_2D.constraints = RigidbodyConstraints2D.FreezeAll;
        
        animator.SetBool("Rotation",false);

        transform.parent = collider.transform;

        if(collider.GetComponent<Enemy>()!=null)
        {
            // isStuckIntoEnemy = true;
        }
        else
        {
            isStuckIntoGround = true;
        }
    }
    
    private void SwordSkillDamage(Enemy enemy)
    {
        player.stats.DoDamage(enemy.GetComponent<CharacterStats>());
        enemy.FreezeTimeFor(0.7f);

        ItemData_Equipment itemData_Equipment = Inventory.Instance.GetEquipment(EquipmentType.Amulet);
        if(itemData_Equipment != null)
        {
            itemData_Equipment.ProcessItemEffects(enemy.transform);
        }
    }


    private void DestroySword() 
    {
        Destroy(this.gameObject);
    }
}
