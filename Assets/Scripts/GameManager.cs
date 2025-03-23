using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Util;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int CurrentShapeType => currentShapeType;
    public ScriptableObjectManager SO => _soManager;

    [SerializeField] private ShapeManager shapeManager;
    [SerializeField] private Transform bottomWallTransform;
    
    private UpdateComp _updateComp;
    private int currentShapeType;
    private ScriptableObjectManager _soManager;

    private float beatTipTotalTime;
    private float generateShapeTotalTime;

    private int _currentStage;
    private int _nextStage = -1;


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
        IntEventSystem.Register(GameEventEnum.CheckStage, OnCheckStage);
        
        _updateComp.DelayAction(StartGame, 2f);
    }

    private void StartGame()
    {
        ChangeStage(0);
        StartOneRound();
    }

    private void OnCheckStage(object _)
    {
        if (_nextStage < 0)
        {
            Debug.Log($"check stage: {_nextStage}");
        }
        else
        {
            Debug.Log($"check stage: {_nextStage}, {shapeManager.DebrisCount}, {SO.GetDataSettings().bugStageCount[_nextStage]}");   
        }
        
        if (_nextStage > 0 && shapeManager.DebrisCount > SO.GetDataSettings().bugStageCount[_nextStage])
        {
            ChangeStage(_nextStage);
        }
    }

    private void ChangeStage(int newStage)
    {
        _currentStage = newStage;
        if (_currentStage < SO.GetDataSettings().bugStageCount.Count - 1)
        {
            _nextStage = _currentStage + 1;
        }
        else
        {
            _nextStage = -1;
        }
        Debug.Log($"change stage to {_currentStage}, next: {_nextStage}");
        IntEventSystem.Send(GameEventEnum.ChangeStage, _currentStage);
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
        _updateComp.DelayAction(FinishOneRound, beatTipTotalTime + generateShapeTotalTime);
    }
    
    private void UpdateOneRoundTime()
    {
        float delay1 = SO.GetDataSettings().BeatStateDelay;
        
        beatTipTotalTime = currentShapeType + delay1;
        
        float dur = SO.GetDataSettings().GenerateShapeDuration;
        int count = SO.GetDataSettings().GenerateShapeCount;
        float fadeDur = SO.GetDataSettings().ShapeFadeDuration;
        float delay2 = SO.GetDataSettings().GenerateStateDelay;
        generateShapeTotalTime = dur * (count - 1) + fadeDur + delay2;
        Debug.Log($"One Round Time: {beatTipTotalTime}, {generateShapeTotalTime}");
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

    private void FinishOneRound()
    {
        if (shapeManager.DebrisCount > SO.GetDataSettings().bugStageCount.Last())
        {
            Debug.Log("Game Over!");
            bottomWallTransform.gameObject.SetActive(false);
            _updateComp.DelayAction(RestartGame, 3f);
        }
        else
        {
            StartOneRound();
        }
    }

    private void RestartGame()
    {
        bottomWallTransform.gameObject.SetActive(true);
        IntEventSystem.Send(GameEventEnum.ClearAllBug, null);
        StartGame();
    }
}