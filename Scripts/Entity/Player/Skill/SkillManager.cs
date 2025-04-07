using UnityEngine;

public class SkillManager : SingletonMonoBehaviour<SkillManager>
{
    public Dash_Skill dash_Skill;
    public Clone_Skill clone_Skill;
    public Sword_Skill sword_Skill;
    public Blackhole_Skill blackhole_Skill;
    public Crystal_Skill crystal_Skill;

    protected override void Awake()
    {
        base.Awake();
        dash_Skill = GetComponent<Dash_Skill>();
        clone_Skill = GetComponent<Clone_Skill>();
        sword_Skill = GetComponent<Sword_Skill>();
        blackhole_Skill = GetComponent<Blackhole_Skill>();
        crystal_Skill = GetComponent<Crystal_Skill>();
    }

    protected override void Start()
    {
        base.Start();
    }
}
