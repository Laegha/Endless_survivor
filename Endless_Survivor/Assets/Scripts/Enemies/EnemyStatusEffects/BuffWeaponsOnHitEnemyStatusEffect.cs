using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffWeaponsOnHitEnemyStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] WeaponStats _statsBuffs;
    [SerializeField] int _maxBuffStacks = 1;
    [Tooltip("Here, wait for external means it will be debuffed when the enemy dies")][SerializeField] WeaponBuffHandler.BuffDurationType _durationType;
    [SerializeField] float _timeDuration = 5f;
    [SerializeField] int _enemyKillsNeeded = 2;
    [SerializeField] ParticleSystem _buffParticles;
    List<WeaponBuffHandler> _activeBuffHandlers = new();

    int _activeStacks;

    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        var buffWeaponsOriginal = original as BuffWeaponsOnHitEnemyStatusEffect;
        _statsBuffs = new WeaponStats(buffWeaponsOriginal._statsBuffs);
        _maxBuffStacks = buffWeaponsOriginal._maxBuffStacks;
        _durationType = buffWeaponsOriginal._durationType;
        _timeDuration = buffWeaponsOriginal._timeDuration;
        _enemyKillsNeeded = buffWeaponsOriginal._enemyKillsNeeded;
        _buffParticles = buffWeaponsOriginal._buffParticles;

        if (_durationType == WeaponBuffHandler.BuffDurationType.WaitForExternal)
            AffectedEnemyControl.EnemyHP.OnDeath += DebuffWeapons;
    }

    public override void EnemyHit()
    {
        base.EnemyHit();
        //if too many buffs, don't buff again
        if (_activeStacks >= _maxBuffStacks)
            return;
        BuffWeapons();
    }

    void BuffWeapons()
    {
        _activeStacks++;
        var buffedWeapons = PlayerControl.pc.WeaponManager.HeldWeapons;
        foreach (var weapon in buffedWeapons)
            weapon.WeaponStats.TemporalStatIncrease(_statsBuffs, false);

        WeaponBuffHandler weaponDebuffHandler = new(buffedWeapons, _statsBuffs, _durationType, _enemyKillsNeeded, _timeDuration, DecreaseStacks, _buffParticles);
        _activeBuffHandlers.Add(weaponDebuffHandler);
        weaponDebuffHandler.callbackOnEnd += () => _activeBuffHandlers.Remove(weaponDebuffHandler);
    }

    void DecreaseStacks()
    {
        _activeStacks--;
    }

    public void DebuffWeapons(EnemyControl placeholder)
    {
        List<WeaponBuffHandler> activeBuffHandlersCopy = new(_activeBuffHandlers);
        foreach (var buffHandler in activeBuffHandlersCopy)
        {
            buffHandler.DebuffWeapons();
        }
    }
}
