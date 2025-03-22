using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    private UpdateComp _updateComp;
    
    
    private void Awake()
    {
        _updateComp = new UpdateComp();
    }

    private void Start()
    {
        _updateComp.DelayAction(StartGame, 2f);
    }

    private void StartGame()
    {
        StartOneRound();
    }

    private int testIndex = 0;
    private void StartOneRound()
    {
        List<int> listShapeType = new List<int>() {1, 3, 4};
        // int type = listShapeType[Random.Range(0, listShapeType.Count)];
        int type = listShapeType[testIndex];
        testIndex = testIndex + 1 == listShapeType.Count ? 0 : testIndex + 1;
        IntEventSystem.Send(GameEventEnum.ShowBeatTip, type);
        
        Test();
    }

    private void Test()
    {
        _updateComp.DelayAction(StartOneRound, 5f);
    }
}