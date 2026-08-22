using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class WeaponBuffHandler
{
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

    public enum BuffDurationType
    {
        ByEnemyKills,
        ByTime,
        WaitForExternal
    }

    public Action callbackOnEnd;
    WeaponBuffData _buffData;

    int _myBuffStacks;

    float _timer;
    int _killledEnemies;

    List<(WeaponAttackManager, int)> _weaponStacks = new();
    List<(WeaponAttackManager, GameObject)> _activeParticles = new();

    public int MyBuffStacks { get { return _myBuffStacks; } }
    public WeaponBuffData BuffData { get { return _buffData; } }
    public WeaponBuffHandler(WeaponBuffData buffData, Action callbackOnEnd = null)
    {
        _buffData = buffData;
        this.callbackOnEnd = callbackOnEnd;
        UpdateWeaponsList();
    }

    public void UpdateWeaponsList()
    {
        foreach (var particle in _activeParticles)
        {
            if (particle.Item1 != null)
                continue;

            GameObject.DestroyImmediate(particle.Item2);
        }
        _activeParticles.RemoveAll(x => x.Item1 == null);
        _weaponStacks.RemoveAll(x => x.Item1 == null);

        foreach (var weapon in PlayerControl.pc.WeaponManager.HeldWeapons)
        {
            if (_weaponStacks.Any(x => x.Item1 == weapon))
                continue;
            _weaponStacks.Add((weapon, 0));

            if (_buffData.BuffParticleSystem == null)
                continue;

            ParticleConfig particlesConfig = new(_buffData.BuffParticleSystem, Vector2.zero, Quaternion.identity, -1, weapon.transform, true, false);
            var createdParticles = ParticleManager.pm.SpawnParticles(particlesConfig);
            _activeParticles.Add((weapon, createdParticles.gameObject));

        }
    }
    public void UpdateAllWeaponsBuffs()
    {
        foreach (var weapon in _weaponStacks)
        {
            UpdateWeaponBuffs(weapon.Item1);
        }
    }
    void UpdateWeaponBuffs(WeaponAttackManager weapon)
    {
        int weaponStackDiff = _myBuffStacks - _weaponStacks.Find(x => x.Item1 == weapon).Item2;
        for (int i = 0; i < Mathf.Abs(weaponStackDiff); i++)
        {
            if (weaponStackDiff < 0)
                DebuffWeapon(weapon);
            else
                BuffWeapon(weapon);
        }
    }
    public void BuffWeapon(WeaponAttackManager buffedWeapon)
    {
        if (buffedWeapon == null) 
            return;
        
        buffedWeapon.WeaponStats.TemporalStatIncrease(_buffData.StatsBuff, false);
    }

    public void DebuffWeapon(WeaponAttackManager debuffedWeapon)
    {
        if (debuffedWeapon == null) 
            return;
        debuffedWeapon.WeaponStats.TemporalStatIncrease(_buffData.StatsBuff, true);
    }
    public bool IncreaseTimer()
    {
        _timer += Time.deltaTime;
        if (_timer <= BuffData.TimeDuration)
            return false;
        _timer = 0;
        return true;
    }
    public bool KilledEnemy()
    {
        _killledEnemies++;
        if (_killledEnemies < _buffData.EnemyKillsNeeded)
            return false;
        _killledEnemies = 0;
        return true;
    }
    public void AddStack()
    {
        _myBuffStacks++;
        UpdateAllWeaponsBuffs();
    }
    public void RemoveStack()
    {
        _myBuffStacks--;
        UpdateAllWeaponsBuffs();
    }

    public void RemoveBuffCompletely()
    {
        foreach (var particle in _activeParticles)
        {
            GameObject.DestroyImmediate(particle.Item2);
        }
    }
}