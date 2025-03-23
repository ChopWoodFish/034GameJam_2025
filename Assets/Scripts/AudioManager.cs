using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Util;

public class AudioManager : MonoBehaviour
{
    // private AudioSource _audioSource;
    private List<AudioSource> _listBgmAudioSource = new List<AudioSource>();
    private AudioSource _effectAudioSource;
    
    
    private void Awake()
    {
        // _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        InitStageBGM();
        InitEffectAudio();
        IntEventSystem.Register(GameEventEnum.ChangeStage, OnChangeStage);
        IntEventSystem.Register(GameEventEnum.PlaySound, OnPlaySound);
    }

    private void OnPlaySound(object param)
    {
        AudioClip ac = param as AudioClip;
        _effectAudioSource.PlayOneShot(ac);
    }

    private void InitEffectAudio()
    {
        _effectAudioSource = this.AddComponent<AudioSource>();
    }

    private void OnChangeStage(object param)
    {
        int stage = (int) param;
        for (int i = 0; i < _listBgmAudioSource.Count; i++)
        {
            var audioSource = _listBgmAudioSource[i];
            audioSource.mute = i != stage;
        }
    }

    private void InitStageBGM()
    {
        var dataSettings = GameManager.Instance.SO.GetDataSettings();
        for (int i = 0; i < dataSettings.bugStageBGM.Count; i++)
        {
            var audioSource = this.AddComponent<AudioSource>();
            audioSource.clip = dataSettings.bugStageBGM[i];
            audioSource.mute = true;
            audioSource.loop = true;
            audioSource.Play();
            _listBgmAudioSource.Add(audioSource);
        }
    }
}