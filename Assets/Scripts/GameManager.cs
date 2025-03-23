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
        IntEventSystem.Register(GameEventEnum.CorrectClick, OnCorrectClick);

        _isInTuto = true;
        _tutoStep = 0;
        
        _updateComp.DelayAction(StartGame, 2f);
    }

    private void OnCorrectClick(object _)
    {
        if (_isInTuto)
        {
            if (_tutoStep == 2)
            {
                _isInTuto = false;
                _tutoStep = -1;   
            }
            else
            {
                _tutoStep++;
            }
        }
        else
        {
            _correctCount--;
        }
    }

    private void StartGame()
    {
        ChangeStage(0);
        StartOneRound();
    }

    private void OnCheckStage(object _)
    {
        // if (_nextStage < 0)
        // {
        //     Debug.Log($"check stage: {_nextStage}");
        // }
        // else
        // {
        //     Debug.Log($"check stage: {_nextStage}, {shapeManager.DebrisCount}, {SO.GetDataSettings().bugStageCount[_nextStage]}");   
        // }
        
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
    
    private void StartOneRound()
    {
        UpdateOneRoundData();
        UpdateOneRoundTime();
        
        ShowBeatTipState();
        _updateComp.DelayAction(GenerateShapeState, beatTipTotalTime);
        _updateComp.DelayAction(FinishOneRound, beatTipTotalTime + generateShapeTotalTime);
    }

    public bool IsInTuto => _isInTuto;
    private bool _isInTuto;
    private int _tutoStep;
    private List<int> _listFixedShapeType = new List<int>();

    private void UpdateOneRoundData()
    {
        List<int> listShapeType = new List<int>() {1, 3, 4};
        
        if (_isInTuto)
        {
            currentShapeType = listShapeType[_tutoStep];
            return;
        }
        
        int type = listShapeType[Random.Range(0, listShapeType.Count)];
        currentShapeType = type;

        if (_correctCount < 0)
        {
            _correctCount = 5;

            _countOffset += Random.Range(1, 3);
            _durOffset -= 0.05f;
            
            Debug.Log($"OneRoundData: update count offset: {_countOffset}, dur offset: {_durOffset}");
        }
    }

    private float _durOffset;
    private float _minGenerateShapeDur = 0.1f;
    private int _countOffset;
    private int _correctCount = 5;  // 每正确点中5个加难度
    
    private void UpdateOneRoundTime()
    {
                
        float delay1 = SO.GetDataSettings().BeatStateDelay;
        
        beatTipTotalTime = currentShapeType + delay1;
        
        float dur = SO.GetDataSettings().GenerateShapeDuration;
        int count = SO.GetDataSettings().GenerateShapeCount;
        float fadeDur = SO.GetDataSettings().ShapeFadeDuration;
        float delay2 = SO.GetDataSettings().GenerateStateDelay;
        if (_isInTuto)
        {
            count = 1;
            _listFixedShapeType.Clear();
            _listFixedShapeType.Add(currentShapeType);
            _durOffset = 0f;
        }

        generateShapeTotalTime = (dur + _durOffset) * (count + _countOffset - 1) + fadeDur + delay2;
        // Debug.Log($"One Round Time: {beatTipTotalTime}, {generateShapeTotalTime}");
    }

    private void ShowBeatTipState()
    {
        IntEventSystem.Send(GameEventEnum.ShowBeatTip, currentShapeType);
    }

    private void GenerateShapeState()
    {
        float dur = SO.GetDataSettings().GenerateShapeDuration;
        if (_isInTuto)
        {
            float delay = 0f;
            for (int i = 0; i < _listFixedShapeType.Count; i++)
            {
                int genType = _listFixedShapeType[i];
                _updateComp.DelayAction(() =>
                {
                    shapeManager.GenerateOneShape(genType);
                }, delay);
                delay += dur;
            }
        }
        else
        {
            int count = SO.GetDataSettings().GenerateShapeCount;
            _updateComp.ScheduleActionAndExecuteImmediately(() =>
            {
                shapeManager.GenerateOneShape();
            }, dur + _durOffset, count + _countOffset);
        }
    }

    private void FinishOneRound()
    {
        if (shapeManager.DebrisCount > SO.GetDataSettings().bugStageCount.Last())
        {
            Debug.Log("Game Over!");
            bottomWallTransform.gameObject.SetActive(false);
            IntEventSystem.Send(GameEventEnum.ClearAllBug, null);
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
        _durOffset = 0;
        _countOffset = 0;
        
        StartGame();
    }
}