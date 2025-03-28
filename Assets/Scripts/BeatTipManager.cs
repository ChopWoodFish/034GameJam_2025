using System;
using UnityEngine;
using Util;

public class BeatTipManager : MonoBehaviour
{
    [SerializeField] private Transform beatTipBox;
    [SerializeField] private Animator beatTipBoxAnimator;
    [SerializeField] private AudioClip beatSound;
    
    private UpdateComp _updateComp;
    

    private void Awake()
    {
        _updateComp = new UpdateComp();
    }
    
    private void Start()
    {
        IntEventSystem.Register(GameEventEnum.ShowBeatTip, OnShowBeatTip);
    }

    private void OnShowBeatTip(object param)
    {
        int shapeType = (int) param;
        _updateComp.ScheduleActionAndExecuteImmediately(ShowOneBeat, 0.8f, shapeType);
    }

    private void ShowOneBeat()
    {
        if (!beatTipBox.gameObject.activeSelf)
        {
            beatTipBox.gameObject.SetActive(true);
        }
        beatTipBoxAnimator.Play("Beat", 0, 0);
        IntEventSystem.Send(GameEventEnum.PlaySound, beatSound);
    }
}