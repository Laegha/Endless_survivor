using System;
using System.Collections.Generic;
using UnityEngine;

public class WeaponBuffHandler
{
    public List<WeaponAttackManager> buffedWeapons;
    public Action callbackOnEnd;
    WeaponBuffData _buffData;
    static Dictionary<WeaponBuffData, int> _buffStacks;
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
    public int BuffCurrentStacks { get { return _buffStacks.ContainsKey(_buffData) ? _buffStacks[_buffData] : 0; } }
    public enum BuffDurationType
    {
        ByEnemyKills,
        ByTime,
        WaitForExternal
    }

    public WeaponBuffHandler(List<WeaponAttackManager> buffedWeapons, WeaponBuffData buffData, Action callbackOnEnd = null)
    {
        this.buffedWeapons = buffedWeapons;
        _buffData = buffData;
        this.callbackOnEnd = callbackOnEnd;

        if (!_buffStacks.ContainsKey(_buffData))
            _buffStacks.Add(_buffData, 0);
        if (_buffStacks[_buffData] > _buffData.BuffMaxStacks)

            BuffWeapons();
        if (_buffData.DurationType == BuffDurationType.WaitForExternal)
            return;
        if (_buffData.DurationType == BuffDurationType.ByEnemyKills)
        {
            EnemySpawnManager.esm.OnEnemySpawned += AddDeathCallbackToEnemy;
            return;
        }
        if (_buffData.DurationType == BuffDurationType.ByTime)
        {
            GameManager.gm.DelayAction(_buffData.TimeDuration, DebuffWeapons, null);
            return;
        }

    }
    public void BuffWeapons()
    {
        foreach (var weapon in buffedWeapons)
        {
            if (weapon == null) continue;
            weapon.WeaponStats.TemporalStatIncrease(_buffData.StatsBuff, false);
            if (_buffData.BuffParticleSystem == null) continue;
            ParticleConfig particlesConfig = new(_buffData.BuffParticleSystem, Vector2.zero, Quaternion.identity, -1, weapon.transform, true, false);
            var createdParticles = ParticleManager.pm.SpawnParticles(particlesConfig);
            _activeParticles.Add(createdParticles.gameObject);
        }
    }

    public void DebuffWeapons()
    {
        foreach (var weapon in buffedWeapons)
        {
            if (weapon == null) continue;
            weapon.WeaponStats.TemporalStatIncrease(_buffData.StatsBuff, true);
        }
        foreach (var particle in _activeParticles)
        {
            GameObject.DestroyImmediate(particle);
        }
        _buffStacks[_buffData]--;
        if(_buffStacks[_buffData] == 0)
            _buffStacks.Remove(_buffData);
        callbackOnEnd?.Invoke();
    }

    void AddDeathCallbackToEnemy(EnemyControl enemy)
    {
        enemy.EnemyHP.OnDeath += IncreaseEnemyKillCounter;
    }

    void IncreaseEnemyKillCounter(EnemyControl placeholder)
    {
        _enemyKillCounter++;
        if (_enemyKillCounter >= _buffData.EnemyKillsNeeded)
        {
            _enemyKillCounter = 0;
            DebuffWeapons();
        }
    }
}