using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill_Controller : MonoBehaviour
{   
    private Player player; 
    private Animator animator;
    private SpriteRenderer spriteRenderer;
    [SerializeField] private float colorLosingSpeed;
    private float cloneTimer;

    [SerializeField] private Transform attack_Check;
    [SerializeField] private float attack_CheckRadius=0.8f;

    private bool canDuplicateClone;
    private float chanceToDuplicate;

    private int FacingDirection;


    private Transform closestEnemy;

    private void Awake() 
    {   
        animator = this.GetComponent<Animator>();
        spriteRenderer = this.GetComponent<SpriteRenderer>();
    }

    private void Update() 
    {
        cloneTimer -= Time.deltaTime;

        if(cloneTimer < 0) 
        {
            spriteRenderer.color = new Color(1,1,1,spriteRenderer.color.a-Time.deltaTime*colorLosingSpeed);
        }

        if (spriteRenderer.color.a <= 0)
        {
            Destroy(this.gameObject);
        }
    }

    public void SetUpClone(Transform newTransform, float cloneDuartion, bool _canAttack,Vector3 _offset, Transform _closestEnemy, bool _canDuplicateClone, float _chanceToDuplicate, Player _player)  
    {   
        if(_canAttack)
        {
            animator.SetInteger("AttackNumber",Random.Range(1,3));
        }

        transform.position = newTransform.position + _offset;
        cloneTimer = cloneDuartion;
        closestEnemy = _closestEnemy;
        canDuplicateClone = _canDuplicateClone;
        chanceToDuplicate = _chanceToDuplicate;

        player = _player;

        FaceToTarget();
    }

    private void FaceToTarget()
    {
        if(closestEnemy != null) 
        {
            if(closestEnemy.position.x <= transform.position.x)
            {
                RotateClone();
                FacingDirection = -1;
            }
        }
        
    }

    private void RotateClone()
    {
        transform.Rotate(0,180,0);
    }


    #region Animation'sTriggers
    
    private void AnimationTrigger() 
    {
        cloneTimer = -0.1f;
    }

    private void DamageCauseTrigger() 
    {   
        Collider2D[] hits = Physics2D.OverlapCircleAll(attack_Check.position,attack_CheckRadius);

        foreach(Collider2D hit in hits) 
        {
            if(hit.GetComponent<Enemy>()!=null)
            {
                player.stats.DoDamage(hit.GetComponent<CharacterStats>());

                if(canDuplicateClone)
                {
                    if(Random.Range(0,100) < chanceToDuplicate)
                    {
                        SkillManager.Instance.clone_Skill.CreateClone(hit.transform, new Vector3(0.5f * FacingDirection, 0));
                    }
                }
            
            }
        }
    }
    #endregion
}
