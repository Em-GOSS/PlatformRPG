using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystal_Skill : Skill
{   
    [SerializeField] private GameObject crystalPrefab;
    [SerializeField] private float crystalExistDuartion;
    private GameObject currentCrystal;

    [Header("Crystal mirage")]
    [SerializeField] private bool cloneInsteadOfCrystal;


    [Header("Explosive Crystal")]
    [SerializeField] private bool canExplode;

    [Header("Moving Crystal")]
    [SerializeField] private bool canMoveToEnemy;
    [SerializeField] private float moveSpeed;

    [Header("Multi Stacking Crystal")]
    [SerializeField] private bool canUseMultiStacks;
    [SerializeField] private int amountOfStacks;
    [SerializeField] private int multiStackCooldown;
    [SerializeField] private float useTimeWindow;
    [SerializeField] private List<GameObject> crystalLeft = new List<GameObject>();


    public override bool TryToUseSkill()
    {
        return base.TryToUseSkill();
    }
    
    protected override void UseSkill()
    {
        base.UseSkill();

        if(TryToUseMultiCrystal())
            return;


        if(currentCrystal == null)
        {
            CreateCrystal();
        }
        else
        {      

            if(canMoveToEnemy)
                return;

            Vector2 playerOriginPos = player.transform.position;

            //ExchangePos
            player.transform.position = currentCrystal.transform.position;
            currentCrystal.transform.position = playerOriginPos;

            if(cloneInsteadOfCrystal)
            {
                SkillManager.Instance.clone_Skill.CreateClone(currentCrystal.transform, Vector3.zero);
                Destroy(currentCrystal);
            }
            else
            {
                //Life Time Over;
                currentCrystal.GetComponent<Crystal_Skill_Controller>()?.CrystalLifeTimeOver();
            }

            
        }
    }

    public void CreateCrystal()
    {
        currentCrystal = Instantiate(crystalPrefab, player.transform.position, Quaternion.identity);
        Crystal_Skill_Controller crystal_Script = currentCrystal.GetComponent<Crystal_Skill_Controller>();
        crystal_Script.SetUpCrystal(crystalExistDuartion, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(currentCrystal.transform), player);
    }

    private bool TryToUseMultiCrystal()
    {
        if(canUseMultiStacks)
        {
            if(crystalLeft.Count > 0)
            {   
                if(crystalLeft.Count == amountOfStacks)
                    Invoke("ResetAbility", useTimeWindow);
                    
                coolDownDuration = 0;
                GameObject crystalToSpawn = crystalLeft[crystalLeft.Count - 1];
                GameObject newCrystal = Instantiate(crystalToSpawn, player.transform.position, Quaternion.identity);

                crystalLeft.Remove(crystalToSpawn);

                newCrystal.GetComponent<Crystal_Skill_Controller>().
                    SetUpCrystal(crystalExistDuartion, canExplode, canMoveToEnemy, moveSpeed, FindClosestEnemy(newCrystal.transform), player);

                if(crystalLeft.Count <= 0)
                {
                    //Cooldown Skill
                    coolDownDuration = multiStackCooldown;
                    //Refill List
                    RefillCrystalList();
                }
            }
            return true;
        }

        return false;
    }

    public void currentCrystalChooseRandomTarget() => currentCrystal.GetComponent<Crystal_Skill_Controller>().ChooseRandomEnemy();
    


    private void RefillCrystalList()
    {   
        int amountToAppend = amountOfStacks - crystalLeft.Count;

        for(int i = 0; i < amountToAppend; i++)
        {
            crystalLeft.Add(crystalPrefab);
        }
    }

    private void ResetAbility()
    {
        if(coolDownTimer > 0)
        {
            return;
        }

        // coolDownTimer = multiStackCooldown;
        RefillCrystalList();
    }

    protected override void Start()
    {
        base.Start();
    }

    protected override void Update()
    {
        base.Update();
    }

}
