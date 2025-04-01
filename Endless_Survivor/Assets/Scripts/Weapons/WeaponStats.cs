using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    [SerializeField] int _range;
    [SerializeField] float _attackSpeed;
    [SerializeField] int _damage;
    int _level;

    public int Range { get { return _range; } }
    public float AttackSpeed { get { return _attackSpeed; } }
    public int Damage { get { return _damage; } }
    public int Level { get { return _level; } }

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
        _range += (int)(ScalingFunctions.WeaponStatIncrease(statsScaling.Range, level) + Random.Range(0f, 1f) * statsScaling.Range);
        _attackSpeed += ScalingFunctions.WeaponStatIncrease(statsScaling.AttackSpeed, level) + Random.Range(0f, .5f) * statsScaling.AttackSpeed;
        _damage += (int)(ScalingFunctions.WeaponStatIncrease(statsScaling.Damage, level) + Random.Range(0f, 1f) * statsScaling.Damage);
    }
    public static int CurrWaveLevel
    { 
        get
        {
            return WaveManager.wm != null ? WaveManager.wm.CurrWave / 2 + 1 : -1;
        }
    }
}
