using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockStrike_Controller : MonoBehaviour
{   
    [SerializeField] private float speed;
    private CharacterStats targetStats;
    private int damage;

    private Animator animator;

    private bool isTrigger;

    public void SetUpThunder(int _damage, CharacterStats _target)
    {
        this.damage = _damage;
        this.targetStats = _target;
    }

    private void Start() 
    {
        animator = this.GetComponentInChildren<Animator>();     
    }

    private void Update()
    {   
        if(isTrigger)
            return;
        if(targetStats == null)
            return;

        transform.position = Vector2.MoveTowards(transform.position, targetStats.transform.position, Time.deltaTime * speed);

        transform.up = transform.position - targetStats.transform.position;
        
        if(Vector2.Distance(transform.position, targetStats.transform.position) < 0.1f)
        {   
            animator.transform.localRotation = Quaternion.identity;
            animator.transform.localPosition = new Vector2(0, 0.5f);

            transform.localRotation = Quaternion.identity;
            transform.localScale = new Vector3(3,3);            

            animator.SetTrigger("Hit");
            isTrigger = true;

            Invoke("DamageAndDestory", 0.2f);
        }
    }

    private void DamageAndDestory()
    {   
        targetStats.ApplyShock(true);
        targetStats.TakeDamage(damage); 
        Destroy(this.gameObject, 0.4f);
    }
}
