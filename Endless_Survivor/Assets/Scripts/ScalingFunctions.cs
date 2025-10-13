using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScalingFunctions
{
    static float _weaponStatIncreaseTrueLevelRoot = 2;
    static int _weaponStatIncreaseInducedLevelReduction = 3;

    static float _enemyHPIncreaseRoot = 1.7f;
    static float _enemyHPIncreaseTraslation = -0.34f;//this makes it so the enemies start to get stronger faster than the weapons since wave 5
    
    public static int CurrWaveLevel
    {
        get
        {
            return WaveManager.wm != null ? WaveManager.wm.CurrWave / 2 + 1 : -1; ;

        }
    }
    public static int EnemyHPIncrease(int level)
    {
        int result = (int)(Mathf.Pow(level, 1 / _enemyHPIncreaseRoot) + _enemyHPIncreaseTraslation);
        return result;
    }

    public static int PlayerHPIncrease(int level, float increaseScale, int initialHP)
    {
        return (int)(increaseScale * level + initialHP);
    }

    public static float WeaponStatIncreaseTrueLevel(float increaseScale, int level)
    {
        float result = increaseScale * Mathf.Pow(level, 1 / _weaponStatIncreaseTrueLevelRoot);
        //float result = increaseScale * Mathf.Log(level+1, _weaponStatIncreaseTrueLevelBase);//+1 because else lvl 1 would be always 0
        return result;
    }
    public static float WeaponStatIncreaseInducedLevel(float increaseScale, int level)
    {
        if (level < _weaponStatIncreaseInducedLevelReduction)
            return (1 / _weaponStatIncreaseInducedLevelReduction * level) * increaseScale;
        else
            return WeaponStatIncreaseTrueLevel(increaseScale, level - _weaponStatIncreaseInducedLevelReduction);
    }
}
