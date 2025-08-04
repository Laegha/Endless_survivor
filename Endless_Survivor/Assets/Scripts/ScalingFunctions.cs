using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScalingFunctions
{
    static float _weaponStatIncreaseTrueLevelBase = 2;
    static float _weaponStatIncreaseInducedLevelBase = 8;
    public static int EnemyHPIncrease(int x)
    {
        return 0;
    }

    public static float WeaponStatIncreaseTrueLevel(float increaseScale, int level)
    {
        float result = increaseScale * Mathf.Log(level+1, _weaponStatIncreaseTrueLevelBase);//+1 because else lvl 1 would be always 0
        return result;
    }
    public static float WeaponStatIncreaseInducedLevel(float increaseScale, int x)
    {
        float result = increaseScale * Mathf.Log(x+1, _weaponStatIncreaseInducedLevelBase);//+1 because else lvl 1 would be always 0
        return result;
    }
}
