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
    float _currProgressGoal = 5;
    int _currIntensityLevel = 1;
    const float uiAnimIncreasePerLevel = 1;

    public float CurrIntensityLevelProgress { get { return _currIntensityLevelProgress; } }
    public int CurrIntensityLevel { get { return _currIntensityLevel; } }
    private void Awake()
    {
        instance = this;
    }

    public void IncreaseIntensityProgress(float progress)
    {
        _currIntensityLevelProgress += progress;
        if (_currIntensityLevelProgress < _currProgressGoal)
        {
            //GameUIManager.uiManager.IntensityUI.ChangeUI(_currIntensityLevelProgress, _currProgressGoal, 0);
            return;
        }
        _currIntensityLevel++;
        _currIntensityLevelProgress = 0;
        //_currProgressGoal = ??
        //GameUIManager.uiManager.IntensityUI.ChangeUI(_currIntensityLevelProgress, _currProgressGoal, uiAnimIncreasePerLevel);
        MapManager.mm.UpdateBiome();
    }

}
