using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill : Skill
{   
    [SerializeField] private int amountOfAttacks;
    [SerializeField] private float cloneAttackCooldown;
    [SerializeField] private float blackholeDuartion;


    [SerializeField] private GameObject blackholePrefab;
    
    [SerializeField] private float maxSize; 
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;

    Blackhole_Skill_Controller currentBlackholeController;

    protected override void UseSkill()
    {
        base.UseSkill();
        GameObject newBlackholePrefab = Instantiate(blackholePrefab, player.transform.position, Quaternion.identity);
        currentBlackholeController = newBlackholePrefab.GetComponent<Blackhole_Skill_Controller>();
        currentBlackholeController.SetUpBlackHole(maxSize, growSpeed, shrinkSpeed ,amountOfAttacks, cloneAttackCooldown, blackholeDuartion);
    }
    public override bool TryToUseSkill()
    {
        return base.TryToUseSkill();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

    public bool SkillCompleted() 
    {
        if(currentBlackholeController == null)
            return false;
        if(currentBlackholeController.canPlayerExitState)
            return true;
        
        return false;
    }

    public float GetBlackholeFinalRadius()
    {
        return maxSize/2;
    }

}
