using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Blackhole_HotKey_Controller : MonoBehaviour
{
    private KeyCode hotKey;

    private SpriteRenderer spriteRenderer;
    private TextMeshProUGUI hotKey_Text;

    private Transform enemyTransform;
    
    private Blackhole_Skill_Controller blackhole_Skill_Controller;

    public void SetUpHotKey(KeyCode newKey ,Transform _enemyTransform, Blackhole_Skill_Controller _blackhole_Skill_Controller)
    {   
        this.enemyTransform = _enemyTransform;
        this.blackhole_Skill_Controller = _blackhole_Skill_Controller;

        hotKey_Text = this.GetComponentInChildren<TextMeshProUGUI>();
        spriteRenderer = this.GetComponentInChildren<SpriteRenderer>();

        this.hotKey = newKey;
        hotKey_Text.text = newKey.ToString();
    }

    public void Update()
    {
        if(Input.GetKeyDown(hotKey))
        {   
            blackhole_Skill_Controller.addNewTarget(enemyTransform);

            hotKey_Text.color = Color.clear;
            spriteRenderer.color = Color.clear;
        }
    }
}
