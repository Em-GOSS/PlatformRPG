using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum TabContentType
{
    Character,
    SkillTree,
    Craft,
    Option
}
public class TabContentUI : MonoBehaviour
{   
    [SerializeField] private TabContentType contentType;
    public virtual void Enter()
    {
        Debug.Log("Enter Tab: " + contentType);
    }
}
