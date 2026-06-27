using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    static readonly float _rangeStatVariation = 1f;
    static readonly float _atkSpeedStatVariation = .05f;
    static readonly float _damageStatVariation = 2f;

    [SerializeField] float _range;
    [SerializeField] float _attackSpeed;
    [SerializeField] float _damage;
    [SerializeField] float _knockback;
    System.Action _onStatsIncrease;
    int _trueLevel;
    int _inducedLevel = 0;

    public float Range { get { return _range; } }
    public float AttackSpeed { get { return _attackSpeed; } }
    public float Damage { get { return _damage; } }
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
    public WeaponStats(float range, float attackSpeed, float damage, float knockback, int trueLevel, int inducedLevel)
    {
        _range = range;
        _attackSpeed = attackSpeed;
        _damage = damage;
        _knockback = knockback;
        _trueLevel = trueLevel;
        _inducedLevel = inducedLevel;
    }

    public void SetTrueLevelStats(WeaponStats statsScaling, int level)
    {
        _trueLevel = level;
        _range += ScalingFunctions.WeaponRangeIncreaseTrueLevel(statsScaling.Range, level) + GetStatVariation(statsScaling.Range, level, _range, _rangeStatVariation);
        _attackSpeed += ScalingFunctions.WeaponAtkSpdIncreaseTrueLevel(statsScaling.AttackSpeed, level) + GetStatVariation(statsScaling.AttackSpeed, level, _attackSpeed, _atkSpeedStatVariation); ;
        _damage += ScalingFunctions.WeaponDamageIncreaseTrueLevel(statsScaling.Damage, level) + GetStatVariation(statsScaling.Damage, level, _damage, _damageStatVariation); ;
    }
    public void InducedLevelUp(WeaponStats statsScaling)
    {
        _inducedLevel++;
        int practicalLevel = _trueLevel + _inducedLevel;
        float rangeIncrease = ScalingFunctions.WeaponRangeIncreaseTrueLevel(statsScaling.Range, practicalLevel) - ScalingFunctions.WeaponRangeIncreaseInducedLevel(statsScaling.Range, practicalLevel);
        rangeIncrease = GetStatVariation(statsScaling.Range, practicalLevel, _range, _rangeStatVariation);
        
        float attackSpeedIncrease = ScalingFunctions.WeaponAtkSpdIncreaseTrueLevel(statsScaling.AttackSpeed, practicalLevel) - ScalingFunctions.WeaponAtkSpdIncreaseInducedLevel(statsScaling.AttackSpeed, practicalLevel);
        attackSpeedIncrease = GetStatVariation(statsScaling.AttackSpeed, practicalLevel, _attackSpeed, _atkSpeedStatVariation);
        
        float damageIncrease = ScalingFunctions.WeaponDamageIncreaseTrueLevel(statsScaling.Damage, practicalLevel) - ScalingFunctions.WeaponDamageIncreaseInducedLevel(statsScaling.Damage, practicalLevel);
        damageIncrease = GetStatVariation(statsScaling.Damage, practicalLevel, _damage, _damageStatVariation);
        StatIncrease(rangeIncrease, attackSpeedIncrease, damageIncrease, 0);
    }
    public void StatIncrease(float range, float attackSpeed, float damage, float knockback)
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
    float GetStatVariation(float increaseScale, int level, float baseStat, float variation)
    {
        return Random.Range(baseStat > variation ? -variation : 0, variation) * increaseScale;
    }
}
