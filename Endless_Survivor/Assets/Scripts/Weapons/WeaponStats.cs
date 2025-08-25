using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    static readonly float _rangeStatVariation = 1f;
    static readonly float _atkSpeedStatVariation = .5f;
    static readonly float _damageStatVariation = 1f;

    [SerializeField] int _range;
    [SerializeField] float _attackSpeed;
    [SerializeField] int _damage;
    int _trueLevel;
    int _inducedLevel = 0;

    public int Range { get { return _range; } }
    public float AttackSpeed { get { return _attackSpeed; } }
    public int Damage { get { return _damage; } }
    public int TrueLevel { get { return _trueLevel; } }
    public int InducedLevel { get { return _inducedLevel; } }

    public WeaponStats(WeaponStats original = null)
    {
        if (original == null)
            return;
        _trueLevel = original._trueLevel;
        _range = original.Range;
        _attackSpeed = original.AttackSpeed;
        _damage = original.Damage;
    }

    public void SetTrueLevelStats(WeaponStats statsScaling, int level)
    {
        _trueLevel = level;
        _range += (int)(ScalingFunctions.WeaponStatIncreaseTrueLevel(statsScaling.Range, level) + Random.Range(0f, _rangeStatVariation) * statsScaling.Range);
        _attackSpeed += ScalingFunctions.WeaponStatIncreaseTrueLevel(statsScaling.AttackSpeed, level) + Random.Range(0f, _atkSpeedStatVariation) * statsScaling.AttackSpeed;
        _damage += (int)(ScalingFunctions.WeaponStatIncreaseTrueLevel(statsScaling.Damage, level) + Random.Range(0f, _damageStatVariation) * statsScaling.Damage);
    }
    public void InducedLevelUp(WeaponStats statsScaling)
    {
        _inducedLevel++;
        _range += (int)(ScalingFunctions.WeaponStatIncreaseInducedLevel(statsScaling.Range, _trueLevel + _inducedLevel) + Random.Range(0f, _rangeStatVariation) * statsScaling.Range);
        _attackSpeed += ScalingFunctions.WeaponStatIncreaseInducedLevel(statsScaling.AttackSpeed, _trueLevel + _inducedLevel) + Random.Range(0f, _atkSpeedStatVariation) * statsScaling.AttackSpeed;
        _damage += (int)(ScalingFunctions.WeaponStatIncreaseInducedLevel(statsScaling.Damage, _trueLevel + _inducedLevel) + Random.Range(0f, _damageStatVariation) * statsScaling.Damage);
    }
    public static int CurrWaveLevel
    { 
        get
        {
            return WaveManager.wm != null ? WaveManager.wm.CurrWave / 2 + 1 : -1;
        }
    }
}
