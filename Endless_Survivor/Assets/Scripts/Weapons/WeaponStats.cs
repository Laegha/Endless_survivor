using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    static readonly float _rangeStatVariation = 1f;
    static readonly float _atkSpeedStatVariation = .15f;
    static readonly float _damageStatVariation = 1f;

    [SerializeField] float _range;
    [SerializeField] float _attackSpeed;
    [SerializeField] int _damage;
    [SerializeField] float _knockback;
    System.Action _onStatsIncrease;
    int _trueLevel;
    int _inducedLevel = 0;

    public float Range { get { return _range; } }
    public float AttackSpeed { get { return _attackSpeed; } }
    public int Damage { get { return _damage; } }
    public float Knockback { get { return _knockback; } }
    public System.Action OnStatIncrease {  get { return _onStatsIncrease; } set { _onStatsIncrease = value; }}
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
        _knockback = original.Knockback;
    }

    public void SetTrueLevelStats(WeaponStats statsScaling, int level)
    {
        _trueLevel = level;
        _range += GetTrueStatIncrease(statsScaling.Range, level, _range, _rangeStatVariation);
        _attackSpeed += GetTrueStatIncrease(statsScaling.AttackSpeed, level, _attackSpeed, _atkSpeedStatVariation);
        _damage += (int) GetTrueStatIncrease(statsScaling.Damage, level, _damage, _damageStatVariation);
    }
    public void InducedLevelUp(WeaponStats statsScaling)
    {
        _inducedLevel++;
        var rangeIncrease = GetInducedStatIncrease(statsScaling.Range, _trueLevel + _inducedLevel, _range, _rangeStatVariation);
        var attackSpeedIncrease = (int)GetInducedStatIncrease(statsScaling.AttackSpeed, _trueLevel + _inducedLevel, _attackSpeed, _atkSpeedStatVariation);
        var damageIncrease = (int)GetInducedStatIncrease(statsScaling.Damage, _trueLevel + _inducedLevel, _damage, _damageStatVariation);
        StatIncrease(rangeIncrease, attackSpeedIncrease, damageIncrease, 0);
    }
    public void StatIncrease(float range, float attackSpeed, int damage, float knockback)
    {
        _range += range;
        _attackSpeed += attackSpeed;
        _damage += damage;
        _knockback += knockback;

    }
    public void TemporalStatIncrease(WeaponStats statIncrease, bool invertStats = false)
    {
        if(!invertStats)
            StatIncrease(statIncrease.Range, statIncrease.AttackSpeed, statIncrease.Damage, statIncrease.Knockback);
        else
            StatIncrease(-statIncrease.Range, -statIncrease.AttackSpeed, -statIncrease.Damage, -statIncrease.Knockback);
    }
    float GetTrueStatIncrease(float increaseScale, int level, float baseStat, float variation)
    {
        return ScalingFunctions.WeaponStatIncreaseTrueLevel(increaseScale, level) + Random.Range(baseStat > variation ? -variation : 0, variation);
    }
    float GetInducedStatIncrease(float increaseScale, int level, float baseStat, float variation)
    {
        return ScalingFunctions.WeaponStatIncreaseInducedLevel(increaseScale, level) + Random.Range(baseStat > variation ? -variation : 0, variation);
    }
}
