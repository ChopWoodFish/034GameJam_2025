using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Util;

public class AudioManager : MonoBehaviour
{
    // private AudioSource _audioSource;
    private List<AudioSource> _listAudioSource = new List<AudioSource>();
    
    
    private void Awake()
    {
        // _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        InitStageBGM();
        IntEventSystem.Register(GameEventEnum.ChangeStage, OnChangeStage);
    }

    private void OnChangeStage(object param)
    {
        int stage = (int) param;
        for (int i = 0; i < _listAudioSource.Count; i++)
        {
            var audioSource = _listAudioSource[i];
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
            _listAudioSource.Add(audioSource);
        }
    }
}