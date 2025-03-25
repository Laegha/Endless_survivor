using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    [SerializeField] int _range;
    [SerializeField] int _attackSpeed;
    [SerializeField] int _damage;

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

    public void ScaleStats(WeaponStats statsScaling)
    {
        _range += ScalingFunctions.WeaponStatIncrease(statsScaling.Range, WaveManager.wm.CurrWave);
        _attackSpeed += ScalingFunctions.WeaponStatIncrease(statsScaling.AttackSpeed, WaveManager.wm.CurrWave);
        _damage += ScalingFunctions.WeaponStatIncrease(statsScaling.Damage, WaveManager.wm.CurrWave);
    }
}
