using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : SingletonMonoBehaviour<UIManager>
{
    [SerializeField] private GameObject uiWindow;

    [SerializeField] private GameObject[] headerTabs;
    [SerializeField] private GameObject[] menuContents;

    [SerializeField] private StatUnit[] statUnits;

    [SerializeField] private UI_ToolTip toolTip;
    [SerializeField] private CraftManager craftManager; 
    private GameObject currentContent;


    protected override void Start()
    {
        base.Start();
        currentContent = menuContents[0];
    }
    private void Update()
    {
        TriggerUI();
    }

    void TriggerUI()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {   
            uiWindow.SetActive(! uiWindow.activeSelf);
            if(uiWindow.activeSelf)
            {
                CallContentEnterEffect();
            }
        }
    }

    public void SwitchActiveContent(int index)
    {

        currentContent.SetActive(false);

        currentContent = menuContents[index];

        currentContent.SetActive(true);
        CallContentEnterEffect();
    }
    public void UpdateAllStatsUI()
    {
        foreach(StatUnit statUnit in statUnits)
        {
            statUnit.UpdateStat();
        }
    }

    private void CallContentEnterEffect()
    {
        currentContent.GetComponent<TabContentUI>()?.Enter();
    }

    public void CallOutToolTip(ItemData_Equipment itemData_Equipment, Vector3 pos)
    {   
        toolTip.GetComponent<RectTransform>().position = pos;
        toolTip.ActiveToolTip(itemData_Equipment);
    }

    public void HideToolTip()
    {
        toolTip.HideToolTip();
    }

    public void UpdateCraftPanel(ItemData itemData)
    {
        craftManager.UpdateCraftPanel(itemData);
    }
}
