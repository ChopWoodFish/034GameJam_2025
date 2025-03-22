using System;
using System.Collections.Generic;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int CurrentShapeType => currentShapeType;
    public ScriptableObjectManager SO => _soManager;

    [SerializeField] private ShapeManager shapeManager;
    
    private UpdateComp _updateComp;
    private int currentShapeType;
    private ScriptableObjectManager _soManager;

    private float beatTipTotalTime;
    private float generateShapeTotalTime;
    
    

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError("repeat Game Manager!");
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        _updateComp = new UpdateComp();
        _soManager = new ScriptableObjectManager();
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
        int type = listShapeType[Random.Range(0, listShapeType.Count)];
        // int type = listShapeType[testIndex];
        currentShapeType = type;
        testIndex = testIndex + 1 == listShapeType.Count ? 0 : testIndex + 1;
        UpdateOneRoundTime();
        
        ShowBeatTipState();
        _updateComp.DelayAction(GenerateShapeState, beatTipTotalTime);
        _updateComp.DelayAction(Test, beatTipTotalTime + generateShapeTotalTime);
    }
    
    private void UpdateOneRoundTime()
    {
        beatTipTotalTime = currentShapeType + 1f;
        
        float dur = SO.GetDataSettings().GenerateShapeDuration;
        int count = SO.GetDataSettings().GenerateShapeCount;
        float fadeDur = SO.GetDataSettings().ShapeFadeDuration;
        generateShapeTotalTime = dur * (count - 1) + fadeDur;
    }

    private void ShowBeatTipState()
    {
        IntEventSystem.Send(GameEventEnum.ShowBeatTip, currentShapeType);
    }

    private void GenerateShapeState()
    {
        float dur = SO.GetDataSettings().GenerateShapeDuration;
        int count = SO.GetDataSettings().GenerateShapeCount;
        float fadeDur = SO.GetDataSettings().ShapeFadeDuration;
        _updateComp.ScheduleActionAndExecuteImmediately(() =>
        {
            shapeManager.GenerateOneShape();
        }, dur, count);
    }

    private void Test()
    {
        StartOneRound();
    }
}