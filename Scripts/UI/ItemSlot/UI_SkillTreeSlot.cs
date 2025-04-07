using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_SkillTreeSlot : MonoBehaviour
{   
    [SerializeField] private string skillName;
    [SerializeField] private string skillDescription;
    [SerializeField] private UI_SkillTreeSlot parentTreeSlot;

    [SerializeField] private Image skillImage;

    public bool unlocked;

    private void OnValidate() 
    {
        this.gameObject.name = "SkillTreeSlot - " + skillName;    
    }

    private void Start() 
    {
        skillImage = GetComponent<Image>();
        skillImage.color = Color.red;
        this.GetComponent<Button>().onClick.AddListener(() => TryToUnlcokSlot());
    }
    public void TryToUnlcokSlot()
    {   
        if(parentTreeSlot != null)  
        {
            if(parentTreeSlot.unlocked == true)
            {
                HeightSkillSlot();
            }
        }
        else
        {
            HeightSkillSlot();
        }
     
    }

    private void HeightSkillSlot()
    {
        skillImage.color = Color.green;
        unlocked = true;
    }


}
