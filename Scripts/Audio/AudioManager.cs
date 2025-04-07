using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{   
    [SerializeField] private AudioSource[] sfx;
    [SerializeField] private AudioSource[] bgm;

    private Dictionary<AudioSource, float> sfxVolumeDic;  
    public bool playBgm;
    private int bgmIndex;
    protected override void Awake()
    {   
        base.Awake();
        BuildSFXVolumeDic();
    }
    private void BuildSFXVolumeDic()
    {   
        sfxVolumeDic = new Dictionary<AudioSource, float>();
        foreach(AudioSource _audio in sfx)
        {
            sfxVolumeDic.Add(_audio, _audio.volume);
        }
    }
    private void Update()
    {
        if(!playBgm)
        {
            StopAllBGM();
        }
        else
        {
            if(!bgm[bgmIndex].isPlaying)
            {
                PlayRandomBGM();
            }
        }
        
    }

  
    public void PlaySFX(int _sfxIndex)
    {
        if(_sfxIndex < sfx.Length)
        {   
            sfx[_sfxIndex].pitch = Random.Range(0.8f, 1.25f);
            sfx[_sfxIndex].volume = sfxVolumeDic[sfx[_sfxIndex]];
            sfx[_sfxIndex].panStereo = 0;
            sfx[_sfxIndex].Play();
        }
    }

    public void PlaySFX(int _sfxIndex, float _maxDistance, Transform _voiceOrigin)
    {          
        if(_sfxIndex < sfx.Length)
        {   
            float panStereoDir = 1;
            if(PlayerManager.Instance.player.transform.position.x >= _voiceOrigin.position.x )
            {
                panStereoDir = -1;
            }

            float distance = Vector2.Distance(PlayerManager.Instance.player.transform.position, _voiceOrigin.position);
            Debug.Log(distance);
            float attenuatedVolume = ( ( _maxDistance - distance)/ _maxDistance) * sfxVolumeDic[sfx[_sfxIndex]];
            sfx[_sfxIndex].panStereo = panStereoDir;
            //sfx[_sfxIndex].pitch = Random.Range(0.8f, 1.25f);
            if(attenuatedVolume < 0)
            {
                sfx[_sfxIndex].volume = 0;
            }
            else
            {
                sfx[_sfxIndex].volume = attenuatedVolume;
            }
            
            sfx[_sfxIndex].Play();
        }
    }

    public void StopSFX(int _sfxIndex)
    {
        sfx[_sfxIndex].Stop();
    }

    public void PlayRandomBGM()
    {
        bgmIndex = Random.Range(0, bgm.Length);
        PlayBGM(bgmIndex);
    }

    public void PlayBGM(int _bgmIndex)
    {
        
        bgmIndex = _bgmIndex;

        StopAllBGM();
        bgm[_bgmIndex].Play();
    }

    public void StopAllBGM()
    {
        for(int i = 0; i < bgm.Length; i++)
        {
            bgm[i].Stop();
        }
    }


}
