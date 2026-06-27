using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScalingFunctions
{
    static float _weaponStatIncreaseTrueLevelRoot = 2;
    static int _weaponStatIncreaseInducedLevelReduction = 3;

    static float _enemyHPIncreaseRoot = 1.7f;
    static float _enemyHPIncreaseTraslation = -0.34f;//this makes it so the enemies start to get stronger faster than the weapons since wave 5

    public static int CurrScalingLevel
    {
        get
        {
            return (int)(IntensityManager.im.CurrIntensityLevel + IntensityManager.im.CurrIntensityLevelProgress / 1000);
            //return WaveManager.wm != null ? WaveManager.wm.CurrWave / 2 + 1 : -1; ;

        }
    }
    public static int EnemyHPIncrease(int level)
    {
        int result = 3 * level;//should scale differently each enemy
        return result;
    }

    public static int PlayerHPIncrease(int level, float increaseScale, int initialHP)
    {
        return (int)(increaseScale * level + initialHP);
    }

    public static float WeaponDamageIncreaseTrueLevel(float increaseScale, int level)
    {
        float y = 0;
        if(0 < level && level <= 4)
        {
            y = level * 2.5f;
        }
        else if(level <= 12)
        {
            y = level * 4.375f - 7.5f;
        }
        else if(level <= 15)
        {
            y = level * 3.33f + 5f;
        }
        else
        {
            y = Mathf.Pow(level, 1 / 2) + 51;
        }
        return y * increaseScale;
    }
    public static float WeaponDamageIncreaseInducedLevel(float increaseScale, int level)
    {
        return WeaponDamageIncreaseTrueLevel(increaseScale, level) / 2;
    }
    public static float WeaponAtkSpdIncreaseTrueLevel(float increaseScale, int level)
    {
        float y = 0;
        if(0 < level && level <= 12)
        {
            y = level * 0.0416f;
        }
        else if(level <= 15)
        {
            y = level * 0.16f + 1.4f;
        }
        else
        {
            y = Mathf.Pow(level, 1 / 2) / 1.3f -2;
        }
        return y * increaseScale;
    }
    public static float WeaponAtkSpdIncreaseInducedLevel(float increaseScale, int level)
    {
        return WeaponAtkSpdIncreaseTrueLevel(increaseScale, level) / 2;
    }
    public static float WeaponRangeIncreaseTrueLevel(float increaseScale, int level)
    {
        return increaseScale * level * 0.6f;
    }
    public static float WeaponRangeIncreaseInducedLevel(float increaseScale, int level)
    {
        return WeaponRangeIncreaseTrueLevel(increaseScale, level) / 2.5f;
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

    public static float EnemyKillIntensityProgress(float enemyHP)
    {
        return enemyHP *  GameManager.gm.WorldConfig.EnemyHPPercentToIntensityIncrease / 100;
    }

    public static int PlayerDamageFormula(int incomingDamage)
    {
        return Mathf.CeilToInt(incomingDamage / Mathf.Clamp(PlayerControl.pc.PlayerStats.Defense, 0.1f, Mathf.Infinity) * (1 + (IntensityManager.im.CurrIntensityLevel * 0.05f)));
    }
}
