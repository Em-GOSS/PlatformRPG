using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skill : MonoBehaviour
{       
    [SerializeField] protected float coolDownDuration;
    public float coolDownTimer;
    
    public Player player{get; private set;}

    protected virtual void Start() 
    {
        player = PlayerManager.Instance.player;
    }

    protected virtual void Update() 
    {
        coolDownTimer -= Time.deltaTime;
    }

    protected virtual void UseSkill() 
    {  
        
    }

    public virtual bool TryToUseSkill() 
    {
        if(coolDownTimer<0)
        {       
            UseSkill();
            coolDownTimer = coolDownDuration;
            return true;
        }
        else
        {   
            Debug.Log("This Skill is under coolingDown");
            return false;
        }
    }

    public bool IsSkillCharingOver()
    {
        if(coolDownTimer < 0)
            return true;
        return false;
    }

    protected virtual Transform FindClosestEnemy(Transform _checkTransform,float _searchRadius = 25)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(_checkTransform.position, _searchRadius);
        float MinDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach(Collider2D hit in hits)
        {    
            Enemy enemy = hit.GetComponent<Enemy>();

            if(enemy != null)
            {
                float distance = Vector2.Distance(hit.transform.position, _checkTransform.position);
                if(distance < MinDistance)
                {
                    MinDistance=distance;
                    closestEnemy=hit.transform;
                }
            }
        }

        return closestEnemy;
    }

}
