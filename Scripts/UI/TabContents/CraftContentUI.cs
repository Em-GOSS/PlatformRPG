using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftContentUI : TabContentUI
{   
    [SerializeField] private CraftManager craftManager;
    public override void Enter()
    {   
        craftManager.InitializeCraftUI();
        base.Enter();
    }
}
