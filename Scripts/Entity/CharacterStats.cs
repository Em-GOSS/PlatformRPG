using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterStats : MonoBehaviour
{   
    private EntityFX entityFX;

    [Header ("Major Stats")]
    public Stat strength; // 1 point increase damge by 1 and crit.power by 1%
    public Stat agility;  // 1 point increase evasion by 1% and crit.chance by 1%
    public Stat intelligence;   // 1 point increase magic damage by 1 and magic resistence by 3
    public Stat vitality;   // 1 point increase health by 3 or 5 points

    [Header("Offsensive Stats")]
    public Stat damage;
    public Stat critChance; 
    public Stat critPower;  //default value 150%

    [Header("Defensive Stats")]
    public Stat maxHealth;
    public Stat armor;
    public Stat evasion;
    public Stat magicResistance;

    [Header("Magic Stat")]
    public Stat fireDamage;
    public Stat iceDamage;
    public Stat lightingDamage;

    [Header("Condition")]
    public bool isIgnited;  //does damage over time 
    public bool isChilled;  //reduce armor by 20%
    public bool isShocked;  //Reduce accuracy by 20%


    [Header ("Condition Timer")]
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    [Header("Condition Duartion")]
    public float ignitedDuartion;
    public float chilledDuartion;
    public float shockedDuartion;
    

    private float ignitedDamageTimer;
    private int ignitedDamage;

    private int shockedDamage;
    [SerializeField] private GameObject shockStrikePrefab;

    [SerializeField] private float ignitedDamageCoolDown;


    public System.Action onHealthChanged;
    public int currentHealth {get; private set;}
    
    protected virtual void Awake()
    {
        critPower.SetDefaultValue(150);
    }
    protected virtual void Start()
    {       
        entityFX = this.GetComponent<EntityFX>();
       
        currentHealth = GetMaxHealthValue();
    }

    protected virtual void Update()
    {
        if(isIgnited)
        {
            ApplyIgnitedDamage();
        }

        if (isChilled)
        {
            chilledTimer -= Time.deltaTime;
            if(chilledTimer < 0)
            {
                isChilled = false;
            }

        }

        if(isShocked)
        {
            shockedTimer -= Time.deltaTime;
            if(shockedTimer < 0)
            {
                isShocked = false;
            }

        }
        
    }

    private void ApplyIgnitedDamage()
    {
        ignitedTimer -= Time.deltaTime;
        ignitedDamageTimer -= Time.deltaTime;
        if (ignitedTimer < 0)
        {
            isIgnited = false;
        }

        if (ignitedDamageTimer < 0)
        {
            ignitedDamageTimer = ignitedDamageCoolDown;

            DecreaseHealthBy(ignitedDamage);

            if (currentHealth < 0)
                Die();
        }
    }

    public virtual void ModifyStatBy(Stat _statToModify, int _modifer, float _duartion)
    {
        StartCoroutine(StatModifyCoroutine(_statToModify, _modifer, _duartion));
    }

    private IEnumerator StatModifyCoroutine(Stat statToModify, int modifer, float duartion)
    {
        statToModify.AddModifer(modifer);

        yield return new WaitForSeconds(duartion);

        statToModify.RemoveModifer(modifer);
    }


    #region physicDamage
    public virtual void DoDamage(CharacterStats _targetStats)
    {
        if (TargetCanAvoidAttack(_targetStats))
        {   
            return;
        }

        //origin damage caculate
        int totalDamage = this.damage.GetValue() + this.strength.GetValue();

        if(CanCrit())
        {
            totalDamage = CaculateCriticalDamage(totalDamage);
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);

        _targetStats.TakeDamage(totalDamage);

        //Only use when we equip the magical weapon
        // DoMagicalDamage(_targetStats);

    }

    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();
        
        if(isShocked) 
            totalEvasion += 20;
        
        if (Random.Range(0, 100) < totalEvasion)
        {   
            Debug.Log("Avoid this Attack");
            return true;
        }

        return false;
    }

    private bool CanCrit()
    {
        int totalCriticalChance = critChance.GetValue() + agility.GetValue();

        if (Random.Range(0, 100) <= totalCriticalChance)
        {
            return true;
        }
        
        return false;
    }

    private int CaculateCriticalDamage(int _damage)
    {
        float totalCriticalPower = (critPower.GetValue() + strength.GetValue()) * 0.01f;

        float criticalDamge =  _damage * totalCriticalPower;

        return Mathf.RoundToInt(criticalDamge);
    }

    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {   
        if(_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() * 0.8f);
        else
            totalDamage -= _targetStats.armor.GetValue(); 

        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

#endregion

#region magicalDamage and alimentApplyment
    public virtual void DoMagicalDamage(CharacterStats _targetStats)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicalDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();
        totalMagicalDamage = CheckTargetResistence(_targetStats, totalMagicalDamage);

        _targetStats.TakeDamage(totalMagicalDamage);


        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;

        AttemptyToApplyAliments(_targetStats, _fireDamage, _iceDamage, _lightingDamage);
    }

    private void AttemptyToApplyAliments(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage)
    {
        bool canApplyIgnited = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChilled = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShocked = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;

        while (!canApplyIgnited && !canApplyChilled && !canApplyShocked)
        {
            if (Random.value < 0.5f && _fireDamage > 0)
            {
                canApplyIgnited = true;
                break;
            }

            if (Random.value < 0.5f && _iceDamage > 0)
            {
                canApplyChilled = true;
                break;
            }

            if (Random.value < 0.5f && _lightingDamage > 0)
            {
                canApplyShocked = true;
                break;
            }
        }

        if (canApplyIgnited)
        {
            _targetStats.SetupIgnitedDamage(Mathf.RoundToInt(_fireDamage * 0.2f));
        }

        if (canApplyShocked)
        {
            _targetStats.SetupShockedDamage(Mathf.RoundToInt(_lightingDamage * 0.5f));
        }

        _targetStats.ApplyAilment(canApplyIgnited, canApplyChilled, canApplyShocked);
    }

    private int CheckTargetResistence(CharacterStats _targetStats, int totalMagicalDamage)
    {
        totalMagicalDamage -= _targetStats.magicResistance.GetValue() + _targetStats.intelligence.GetValue() * 3;
        totalMagicalDamage = Mathf.Clamp(totalMagicalDamage, 0, int.MaxValue);
        return totalMagicalDamage;
    }

    public void ApplyAilment(bool _isIgnited, bool _isChilled, bool _isShocked)
    {   
        bool canApplyIgnited = !isIgnited && !isChilled && !isShocked;
        bool canApplyChilled = !isIgnited && !isChilled && !isShocked;
        bool canApplyShocked = !isIgnited && !isChilled;

        

        if(_isIgnited && canApplyIgnited)
        {   
            isIgnited = _isIgnited;

            ignitedTimer = ignitedDuartion;
            entityFX.IgnitedFxFor(ignitedTimer, ignitedDamageCoolDown);
        }

        if(_isChilled && canApplyChilled)
        {   
            isChilled = _isChilled;

            chilledTimer = chilledDuartion;
            entityFX.ChilledFxFor(chilledTimer);

            this.GetComponent<Entity>().SlowEntityBy(0.2f, chilledTimer);
        }

        if(_isShocked && canApplyShocked)
        {   
            if(!isShocked)
            {
                ApplyShock(_isShocked);
            }
            else
            {
                if (this.GetComponent<Player>() != null)
                    return;

                //thunder strike
                ShockThunderHit();
            }

        }
           
    }

    public void ApplyShock(bool _isShocked)
    {   
        if(isShocked)
            return;

        isShocked = _isShocked;

        shockedTimer = shockedDuartion;
        entityFX.ShockedFxFor(shockedDuartion);
    }

    private void ShockThunderHit()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, 12);
        float MinDistance = Mathf.Infinity;
        Transform closestEnemy = null;


        foreach (Collider2D hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();

            if (enemy != null && enemy != this.GetComponent<Enemy>())
            {
                float distance = Vector2.Distance(hit.transform.position, transform.position);
                if (distance < MinDistance)
                {
                    MinDistance = distance;
                    closestEnemy = hit.transform;
                }
            }
        }

        if (closestEnemy == null)
        {
            closestEnemy = this.transform;
        }


        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);
            newShockStrike.GetComponent<ShockStrike_Controller>().SetUpThunder(shockedDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

#endregion

    public virtual void Heal(int _healValue)
    {
        IncreaseHealthBy(_healValue);
    }

    public virtual void TakeDamage(int _damage)
    {
        DecreaseHealthBy(_damage);

        this.GetComponent<Entity>().DamageImpact();
        entityFX.StartCoroutine("FlashFX");

        if(currentHealth < 0)
        {
            Die();    
        }
    }

    protected virtual void Die()
    {
       
    }


    protected virtual void IncreaseHealthBy(int _healValue)
    {
        currentHealth += _healValue;
        if(currentHealth >= maxHealth.GetValue())
            currentHealth = maxHealth.GetValue();

        if(onHealthChanged != null)
            onHealthChanged();    
    }
    protected virtual void DecreaseHealthBy(int _damage)
    {   
        currentHealth -= _damage;

        if(onHealthChanged != null)
            onHealthChanged();
    }

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    public void SetupIgnitedDamage(int _damage) => ignitedDamage = _damage;
    public void SetupShockedDamage(int _damage) =>shockedDamage = _damage;
}
