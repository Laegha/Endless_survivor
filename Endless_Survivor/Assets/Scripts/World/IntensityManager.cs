using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensityManager : MonoBehaviour
{
    static IntensityManager instance;
    public static IntensityManager im
    {
        get 
        {
            return instance;
        }
    }
    float _currIntensityLevelProgress = 0;
    float _currProgressGoal;
    int _currIntensityLevel = 0;
    const float uiAnimIncreasePerLevel = .5f;
    Action _onLevelIncrease;

    public float CurrIntensityLevelProgress { get { return _currIntensityLevelProgress; } }
    public int CurrIntensityLevel { get { return _currIntensityLevel; } }
    public Action OnLevelIncrease {  get { return _onLevelIncrease; } set { _onLevelIncrease = value; } }
    private void Awake()
    {
        instance = this;
    }
    private void Start()
    {
        _currProgressGoal = GameManager.gm.WorldConfig.InitialInstensityGoal;
        GameUIManager.uiManager.IntensityUI.ChangeUI(_currIntensityLevelProgress, _currProgressGoal, 0);
    }
    public void IncreaseIntensityProgress(float progress)
    {
        _currIntensityLevelProgress += progress;
        if (_currIntensityLevelProgress < _currProgressGoal)
        {
            GameUIManager.uiManager.IntensityUI.ChangeUI(_currIntensityLevelProgress, _currProgressGoal, 0);
            return;
        }
        _currIntensityLevel++;
        _currIntensityLevelProgress = 0;
        _currProgressGoal += GameManager.gm.WorldConfig.IntensityGoalIncrease;
        GameUIManager.uiManager.IntensityUI.ChangeUI(_currIntensityLevelProgress, _currProgressGoal, uiAnimIncreasePerLevel);
        GameUIManager.uiManager.DisplayUIMessage(GameManager.gm.WorldConfig.IntensityIncreaseMessageInfo);
        int levelsTillNewBiome = _currIntensityLevel % GameManager.gm.WorldConfig.IntensityLevelsForNewBiome;
        if(levelsTillNewBiome == 0)
            MapManager.mm.UpdateBiome();
        _onLevelIncrease?.Invoke();
    }

}
