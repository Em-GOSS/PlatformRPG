using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar_UI : MonoBehaviour
{
    private Entity entity;
    private RectTransform rectTransform;
    private Slider slider;  
    private CharacterStats stats;

    private void Start() 
    {
        entity = this.GetComponentInParent<Entity>();
        rectTransform = this.GetComponent<RectTransform>();
        slider = this.GetComponent<Slider>();
        stats = this.GetComponentInParent<CharacterStats>();

        entity.onFlipped += FlipUI;
        stats.onHealthChanged += UpdateHealthUI;

        //Initialize
        UpdateHealthUI();
    }

    public void UpdateHealthUI()
    {
        slider.maxValue = stats.GetMaxHealthValue();
        slider.value = stats.currentHealth;
    }

    private void FlipUI() => rectTransform.Rotate(0, 180, 0);

    private void OnDisable()
    {
        entity.onFlipped -= FlipUI;
        stats.onHealthChanged -= UpdateHealthUI;
    }
}
