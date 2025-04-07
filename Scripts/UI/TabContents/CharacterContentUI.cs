using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterContentUI : TabContentUI
{
    [SerializeField] private Image playerImage;
    [SerializeField] private float playerFadeSpeed;

    public override void Enter()
    {   
        if(playerImage.IsActive() == false)
            return;
        base.Enter();
        playerImage.fillAmount = 0;
        StartCoroutine(PlayerUIFadeIn());
    }

    private IEnumerator PlayerUIFadeIn()
    {     

        while(!Mathf.Approximately(playerImage.fillAmount, 1.0f))
        {   
            playerImage.fillAmount += playerFadeSpeed * Time.deltaTime;
            yield return null;
        }
    }

}
