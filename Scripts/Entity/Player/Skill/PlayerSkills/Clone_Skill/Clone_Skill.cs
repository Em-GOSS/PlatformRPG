using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clone_Skill : Skill
{   
    [Header("Clone Info")]
    [SerializeField] private GameObject clonePrefab;
    [SerializeField] private float cloneDuartion;
    [Space]
    [SerializeField] private bool _canAttack;
    [SerializeField] private float target_NoticeRadius = 16f;

    [SerializeField] private bool createCloneOnDashStart;
    [SerializeField] private bool createCloneOnDashOver;
    [SerializeField] private bool createCloneOnCounterAttack;

    [Header("Clone Duplicate")]
    [SerializeField] private bool canDuplicateClone;
    [SerializeField] private float chanceToDuplicate;

    [Header("Crystal Instead Of Clone")]
    public bool crystalInsteadOfClone;

    public void CreateClone(Transform instantiate_Pos, Vector3 _offset)
    {   
        if(crystalInsteadOfClone)
        {
            SkillManager.Instance.crystal_Skill.CreateCrystal();
            return;
        }

        GameObject newClone = Instantiate(clonePrefab);
        Clone_Skill_Controller clone_Skill_Controller = newClone.GetComponent<Clone_Skill_Controller>();
        clone_Skill_Controller.SetUpClone
            (instantiate_Pos, cloneDuartion, _canAttack, _offset, FindClosestEnemy(newClone.transform, target_NoticeRadius), canDuplicateClone, chanceToDuplicate,player);
    }

    public void CreateCloneOnDashStart()
    {
        if(createCloneOnDashStart)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnDashOver()
    {
        if(createCloneOnDashOver)
            CreateClone(player.transform, Vector3.zero);
    }

    public void CreateCloneOnCounterAttack(Transform hitTransform)
    {   

        float yOffset = player.transform.position.y - hitTransform.position.y;
        if(createCloneOnCounterAttack)
            StartCoroutine(CreateCloneWithDelay(hitTransform, new Vector2(2*player.FacingDirection, yOffset)));
            
    }

    private IEnumerator CreateCloneWithDelay(Transform _transform, Vector2 _offset)
    {
        yield return new WaitForSeconds(0.4f);
            CreateClone(_transform, _offset);
    }
}
