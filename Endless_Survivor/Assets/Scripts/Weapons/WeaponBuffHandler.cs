using System;
using System.Collections.Generic;
using UnityEngine;

class WeaponBuffHandler
{
    public List<WeaponAttackManager> buffedWeapons;
    public WeaponStats statsBuff;
    int _enemyKillsNeeded;
    float _timeDuration;
    public Action callbackOnEnd;
    ParticleSystem _buffParticleSystem;
    List<GameObject> _activeParticles = new();
    //Add particles to buffed weapons. multiple buffs of the same type shouldn't stack particles?

    /// <summary>
    ///active stacks should go here instead of the scripts that are using this. 
    ///when a new buff handler is created, you tell it how many is the maxStacks
    ///the other script should create a buff handler on start, then instead of creating new ones each time the stats should be buffed
    ///it tells the buff handler "hey, you should buff now+
    ///THERE'S A PROBLEM WITH WEAPON CHANGES!!!
    ///if too many stacks, the handler simply doesn't care
    ///when a buff should end, activeStacks is decreased by one
    ///if there are no more stacks destroy particles and stop decreasing timer or whatever, but don't destroy the handler
    /// </summary>

    float _enemyKillCounter = 0;
    public enum BuffDurationType
    {
        ByEnemyKills,
        ByTime,
        WaitForExternal
    }

    public WeaponBuffHandler(List<WeaponAttackManager> buffedWeapons, WeaponStats statsBuff, BuffDurationType durationType, int enemyKillsNeeded, float timeDuration, Action callbackOnEnd = null, ParticleSystem buffParticleSystem = null)
    {
        this.buffedWeapons = buffedWeapons;
        this.statsBuff = statsBuff;
        _enemyKillsNeeded = enemyKillsNeeded;
        _timeDuration = timeDuration;
        this.callbackOnEnd = callbackOnEnd;
        _buffParticleSystem = buffParticleSystem;
        BuffWeapons();
        if (durationType == BuffDurationType.WaitForExternal)
            return;
        if(durationType == BuffDurationType.ByEnemyKills)
        {
            EnemySpawnManager.esm.OnEnemySpawned += AddDeathCallbackToEnemy;
            return;
        }
        if(durationType == BuffDurationType.ByTime)
        {
            GameManager.gm.DelayAction(_timeDuration, DebuffWeapons, null);
            return;
        }

    }
    public void BuffWeapons()
    {
        foreach (var weapon in buffedWeapons)
        {
            if (weapon == null) continue;
            weapon.WeaponStats.TemporalStatIncrease(statsBuff, false);
            if (_buffParticleSystem == null) continue;
            ParticleConfig particlesConfig = new(_buffParticleSystem, Vector2.zero, Quaternion.identity, -1, weapon.transform, true, false);
            var createdParticles = ParticleManager.pm.SpawnParticles(particlesConfig);
            _activeParticles.Add(createdParticles.gameObject);
        }
    }

    public void DebuffWeapons()
    {
        foreach (var weapon in buffedWeapons)
        {
            if (weapon == null) continue;
            weapon.WeaponStats.TemporalStatIncrease(statsBuff, true);
        }
        foreach(var particle in _activeParticles)
        {
            GameObject.DestroyImmediate(particle);
        }
        callbackOnEnd?.Invoke();
    }

    void AddDeathCallbackToEnemy(EnemyControl enemy)
    {
        enemy.EnemyHP.OnDeath += IncreaseEnemyKillCounter;
    }

    void IncreaseEnemyKillCounter(EnemyControl placeholder)
    {
        _enemyKillCounter++;
        if(_enemyKillCounter >= _enemyKillsNeeded)
        {
            _enemyKillCounter = 0;
            DebuffWeapons();
        }
    }
}