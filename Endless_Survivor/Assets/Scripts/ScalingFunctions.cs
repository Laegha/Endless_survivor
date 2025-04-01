using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ScalingFunctions
{
    public static int EnemyHPIncrease(int x)
    {
        return 0;
    }

    public static float WeaponStatIncrease(float increaseScale, int x)
    {
        float result = increaseScale * Mathf.Log(x, 2);
        return result;
    }

}
