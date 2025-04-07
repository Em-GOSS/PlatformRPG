using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
public class Option_VolumeSlider : MonoBehaviour
{      
    [SerializeField] private Slider volumeSlider;
    [SerializeField] private string parameter;


    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float multipler;

    public void SliderValue(float _value)
    {
        audioMixer.SetFloat(parameter, _value);
    }

    

   
}
