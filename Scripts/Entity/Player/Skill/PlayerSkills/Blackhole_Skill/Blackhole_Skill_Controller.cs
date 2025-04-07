using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blackhole_Skill_Controller : MonoBehaviour
{   

    private float maxSize; 
    private float growSpeed;
    private float shrinkSpeed;
    private float blackholeTimer;

    private bool canGrow = true ;
    private bool canShrink;
    private bool canAttackReleased = false;
    private bool playerCanDisappear = true;
    private bool canCreateHotKeys = true;


    private int amountOfAttacks = 4;
    private float cloneAttackCooldown = .3f;
    private float cloneAttackTimer;
    [SerializeField] List<Transform> targets = new List<Transform>();
    List<GameObject> CreatedHotKeyList = new List<GameObject>();

    //HotKey
    [SerializeField] private List<KeyCode> keyCodeList;
    [SerializeField] GameObject hotKeyPrefab;

    public bool canPlayerExitState {get; private set;}
    

    public void SetUpBlackHole(float _maxSize, float _growSpeed, float _shrinkSpeed, int _amountOfAttacks, float _cloneAttackCooldown, float _blackholeDuartion
    )
    {
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        amountOfAttacks = _amountOfAttacks;
        cloneAttackCooldown = _cloneAttackCooldown;

        blackholeTimer = _blackholeDuartion;

        if(SkillManager.Instance.clone_Skill.crystalInsteadOfClone)
            playerCanDisappear = false;
    }



    private void Update()
    {
        cloneAttackTimer -= Time.deltaTime;
        blackholeTimer -= Time.deltaTime;
    
        if(blackholeTimer < 0)
        {   
            blackholeTimer = Mathf.Infinity;

            if(targets.Count != 0)
                ReleaseCloneAttack();
            else
                FinishBlackholeAbility();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {   
            if(targets.Count != 0)
                ReleaseCloneAttack();
        }

        CloneAttackLogic();

        //Grow and Shirk
        if (canGrow && !canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(maxSize, maxSize), growSpeed * Time.deltaTime);
        }

        if (canShrink)
        {
            transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(-1, -1), shrinkSpeed * Time.deltaTime);

            if (transform.localScale.x <= 0)
                Destroy(this.gameObject);
        }

    }

    private void ReleaseCloneAttack()
    {   
        DestoryAllHotKey();
        canAttackReleased = true;
        canCreateHotKeys = false;
        if(playerCanDisappear)
        {
            playerCanDisappear = false;
            PlayerManager.Instance.player.entityFX.MakeTransparent(true);
        }       
    }

    private void CloneAttackLogic()
    {
        if (cloneAttackTimer < 0 && canAttackReleased && amountOfAttacks > 0 )
        {
            cloneAttackTimer = cloneAttackCooldown;

            int randomIndex = Random.Range(0, targets.Count);

            float cloneCreateOffset_x;

            if (Random.Range(0, 100) < 50)
                cloneCreateOffset_x = 2;
            else
                cloneCreateOffset_x = -2;

            if(SkillManager.Instance.clone_Skill.crystalInsteadOfClone)
            {
                SkillManager.Instance.crystal_Skill.CreateCrystal();
                SkillManager.Instance.crystal_Skill.currentCrystalChooseRandomTarget();
            }
            else
            {
                SkillManager.Instance.clone_Skill.CreateClone(targets[randomIndex], new Vector3(cloneCreateOffset_x, 0));
            }

           

            amountOfAttacks--;
            if (amountOfAttacks <= 0)
            {
                Invoke("FinishBlackholeAbility",0.5f);
            }
        }
    }

    private void FinishBlackholeAbility()
    {   
        DestoryAllHotKey();
        canShrink = true;
        canAttackReleased = false;
        canPlayerExitState = true;    
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if(collision.GetComponent<Enemy>() != null)
        {
            collision.GetComponent<Enemy>().FreezeTime(true);
            if(canCreateHotKeys)
                CreateHotKey(collision);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) => collision.GetComponent<Enemy>()?.FreezeTime(false);

    private void CreateHotKey(Collider2D collision)
    {   
        if(keyCodeList.Count <= 0)
        {
            Debug.LogWarning("Not enough keycode!");
            return;
        }
        GameObject newHotKey = Instantiate(hotKeyPrefab, collision.transform.position + new Vector3(0,2), Quaternion.identity);

        CreatedHotKeyList.Add(newHotKey);

        KeyCode choosenHotKey = keyCodeList[Random.Range(0, keyCodeList.Count)];
        keyCodeList.Remove(choosenHotKey);

        Blackhole_HotKey_Controller newHotKeyController = newHotKey.GetComponent<Blackhole_HotKey_Controller>();
            
        newHotKeyController.SetUpHotKey(choosenHotKey, collision.transform, this);
    }

    private void DestoryAllHotKey()
    {
        if(CreatedHotKeyList.Count <= 0)
            return;

        foreach(GameObject hotkey in CreatedHotKeyList)
        {
            Destroy(hotkey);
        }
    } 


    public void addNewTarget(Transform targetTransform)
    {
        targets.Add(targetTransform);
    }
}
