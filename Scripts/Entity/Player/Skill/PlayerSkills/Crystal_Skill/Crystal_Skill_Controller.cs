using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill_Controller : MonoBehaviour
{   
    private Player player;
    private Animator animator => this.GetComponent<Animator>();
    private CircleCollider2D circleCollider2D => this.GetComponent<CircleCollider2D>();
    private float crystalExistTimer;

    
    private bool canGrow = false;
    [SerializeField] private float growSpeed;
    private bool canExlode;
    private bool canMoveToEnemy;
    private float moveSpeed;
 
    private Transform closestTarget;

    [SerializeField] private LayerMask whatIsEnemy;

    public void SetUpCrystal(float _crystalExistDuartion, bool _canExplode, bool _canMoveToEnemy, float _moveSpeed, Transform _closestTarget, Player _player)
    {
        crystalExistTimer = _crystalExistDuartion;
        canExlode = _canExplode; 
        canMoveToEnemy = _canMoveToEnemy;
        moveSpeed = _moveSpeed;
        closestTarget = _closestTarget;
        player = _player;
    }

    void Update() 
    {
        crystalExistTimer -= Time.deltaTime;

        if(crystalExistTimer <= 0)
        {
            CrystalLifeTimeOver();
        }

        if(canMoveToEnemy)
        {
            if(closestTarget != null)
            {
                transform.position = Vector2.MoveTowards(this.transform.position, closestTarget.transform.position, Time.deltaTime * moveSpeed);

                if(Vector2.Distance(transform.position, closestTarget.position) < 0.8)
                    CrystalLifeTimeOver();
            }
            
        }


        if(canGrow)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(3,3), growSpeed * Time.deltaTime);
        }
    }

    public void CrystalLifeTimeOver()
    {
        if (canExlode)
        {   
            canGrow = true;
            animator.SetTrigger("Explode");
        }
        else
        {
            SelfDestory();
        }
    }

    void SelfDestory()
    {
        Destroy(this.gameObject);
    }


    private void AnimationExplodeEvent() 
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, circleCollider2D.radius);

        foreach(Collider2D hit in hits) 
        {
            if(hit.GetComponent<Enemy>() != null)
            {
                player.stats.DoMagicalDamage(hit.GetComponent<CharacterStats>());

                ItemData_Equipment itemData_Equipment = Inventory.Instance.GetEquipment(EquipmentType.Amulet);
                
                if(itemData_Equipment != null)
                {
                    itemData_Equipment.ProcessItemEffects(hit.transform);
                }
            }
        }
    }

    public void ChooseRandomEnemy()
    {   
        float radius = SkillManager.Instance.blackhole_Skill.GetBlackholeFinalRadius();

        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, radius, whatIsEnemy);

        if(hits.Length > 0)
        {
            Transform randomTarget = hits[Random.Range(0, hits.Length)].transform;
            //Set the Target to random
            closestTarget = randomTarget;
        }
       
    }
}

