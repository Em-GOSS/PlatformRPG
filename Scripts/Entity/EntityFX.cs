using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class EntityFX : MonoBehaviour
{   


    [Header("Screen Shake")]
    [SerializeField] private CinemachineImpulseSource screenShake;
    [SerializeField] private float shakeMultipler;
    [SerializeField] private Vector3 shakePower;

    private SpriteRenderer spriteRenderer_Entity;

    private Material originMaterial;

    [SerializeField] private Material flashMaterial;
    [SerializeField] private float flashDuration;

    [Header("Aliment Colors")]
    [SerializeField] private Color[] ignitedColor;
    [SerializeField] private Color chilledColor;
    [SerializeField] private Color[] shockedColor; 


    private void Awake() 
    {
        spriteRenderer_Entity = this.GetComponentInChildren<SpriteRenderer>();
        screenShake = this.GetComponent<CinemachineImpulseSource>();
        if(spriteRenderer_Entity!=null) 
        {
            originMaterial=spriteRenderer_Entity.material;
        }
    }

    public void DoScreenShake()
    {
        screenShake.m_DefaultVelocity = new Vector3(shakePower.x * PlayerManager.Instance.player.FacingDirection, shakePower.y, shakePower.z) * shakeMultipler;
        screenShake.GenerateImpulse();
    }

    private IEnumerator FlashFX() 
    {
        spriteRenderer_Entity.material = flashMaterial;
        
        Color originalColor = spriteRenderer_Entity.color;

        yield return new WaitForSeconds(flashDuration); 

        spriteRenderer_Entity.material = originMaterial;
    }

    private void RedBlink()
    {   
        spriteRenderer_Entity.color = spriteRenderer_Entity.color == Color.white ? Color.red : Color.white;
    } 

    private void CancelColorChange() 
    {
        CancelInvoke();
        spriteRenderer_Entity.color = Color.white;
    }   

    
    public void MakeTransparent(bool _transparent)
    {
        if(_transparent)
            spriteRenderer_Entity.color = Color.clear;
        else
            spriteRenderer_Entity.color = Color.white;
    }
#region elementFx
    public void IgnitedFxFor(float _seconds, float damageWindow)
    {
        InvokeRepeating("IgniteColorFx", 0, damageWindow / 2);
        Invoke("CancelColorChange", _seconds);
    }

    public void ChilledFxFor(float _seconds)
    {
        spriteRenderer_Entity.color = chilledColor;
        Invoke("CancelColorChange", _seconds);
    }

    public void ShockedFxFor(float _seconds)
    {
        InvokeRepeating("ShockColorfx", 0 ,0.25f);
        Invoke("CancelColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        if(spriteRenderer_Entity.color != ignitedColor[0])
            spriteRenderer_Entity.color = ignitedColor[0];
        else
            spriteRenderer_Entity.color = ignitedColor[1];
    }

    private void ShockColorfx()
    {
        if(spriteRenderer_Entity.color != shockedColor[0])
            spriteRenderer_Entity.color = shockedColor[0];
        else
            spriteRenderer_Entity.color = shockedColor[1];
    }

#endregion
}
