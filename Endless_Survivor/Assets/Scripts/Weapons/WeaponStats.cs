using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    [SerializeField] int _range;
    [SerializeField] int _attackSpeed;
    [SerializeField] int _damage;
    int _level;

    public int Range { get { return _range; } }
    public int AttackSpeed { get { return _attackSpeed; } }
    public int Damage { get { return _damage; } }

    public WeaponStats(WeaponStats original = null)
    {
        if (original == null)
            return;
        _range = original.Range;
        _attackSpeed = original.AttackSpeed;
        _damage = original.Damage;
    }

    public void ScaleStats(WeaponStats statsScaling, int level)
    {
        _level = level;
        _range += ScalingFunctions.WeaponStatIncrease(statsScaling.Range, level);
        _attackSpeed += ScalingFunctions.WeaponStatIncrease(statsScaling.AttackSpeed, level);
        _damage += ScalingFunctions.WeaponStatIncrease(statsScaling.Damage, level);
    }
    public static int CurrWaveLevel
    { 
        get
        {
            return WaveManager.wm.CurrWave / 2 + 1;
        }
    }
}
