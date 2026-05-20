using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ExceedItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] WeaponStats _statBuffOnAttackCoordination;
    [SerializeField] ParticleSystem _onAttackCoordinationParticles;
    [SerializeField] float _particlesDuration = 1;
    Dictionary<WeaponAttackManager, float> _weaponsInBuffLapse = new();
    readonly float _buffLapseDuration = 0.05f;
    readonly float _buffTimeDuration = 2f;
    Dictionary<WeaponAttackManager, float> _buffedWeapons = new();
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var exceedOriginal = original as ExceedItemBehaviour;
        _statBuffOnAttackCoordination = new WeaponStats(exceedOriginal._statBuffOnAttackCoordination);
        _onAttackCoordinationParticles = exceedOriginal._onAttackCoordinationParticles;
        _particlesDuration = exceedOriginal._particlesDuration;
        behaviourManager.onAttack += PutWeaponInBuffLapse;
        behaviourManager.onUpdate += CheckForCoordinatedWeapons;
        behaviourManager.onUpdate += DecreaseBuffLapseTime;
        behaviourManager.onUpdate += DecreaseActiveBuffTime;
    }
    void CheckForCoordinatedWeapons()
    {
        if (_weaponsInBuffLapse.Count <= 1)
            return;
        var weaponsInBuffLapse = new List<WeaponAttackManager>(_weaponsInBuffLapse.Keys);
        foreach (var weapon in weaponsInBuffLapse)
        {
            if (_buffedWeapons.ContainsKey(weapon))
                continue;
            _buffedWeapons.Add(weapon, _buffTimeDuration);
            //Increase weapon stats
            weapon.WeaponStats.TemporalStatIncrease(_statBuffOnAttackCoordination, false);
            //generate particles
            ParticleConfig exceedParticleConfig = new(_onAttackCoordinationParticles, weapon.transform.position, Quaternion.identity, _particlesDuration, weapon.transform, true, false);
            ParticleManager.pm.SpawnParticles(exceedParticleConfig);
        }
    }
    void PutWeaponInBuffLapse(WeaponAttackManager weapon)
    {
        if (_buffedWeapons.ContainsKey(weapon))
            return;
        if(_weaponsInBuffLapse.ContainsKey(weapon))
            _weaponsInBuffLapse[weapon] = _buffLapseDuration;
        else
            _weaponsInBuffLapse.Add(weapon, _buffLapseDuration);
    }

    void DecreaseBuffLapseTime()
    {
        var weaponsInBuffLapse = new List<WeaponAttackManager>(_weaponsInBuffLapse.Keys);
        foreach(var weapon in weaponsInBuffLapse)
        {
            _weaponsInBuffLapse[weapon] -= Time.deltaTime;
            if (_weaponsInBuffLapse[weapon]  <= 0)
                _weaponsInBuffLapse.Remove(weapon);
        }
    }
    void DecreaseActiveBuffTime()
    {
        var buffedWeapons = new List<WeaponAttackManager>(_buffedWeapons.Keys);
        foreach(var weapon in buffedWeapons)
        {
            _buffedWeapons[weapon] -= Time.deltaTime;
            if (_buffedWeapons[weapon] <= 0)
            {
                //Decrease stats
                RevertStatIncrease(weapon);
            }
        }
    }
    void RevertStatIncrease(WeaponAttackManager weapon)
    {

        weapon.WeaponStats.TemporalStatIncrease(_statBuffOnAttackCoordination, true);
        _buffedWeapons.Remove(weapon);
    }
    public override void RemoveBehaviour()
    {
        foreach(var weapon in _buffedWeapons.Keys)
        {
            RevertStatIncrease(weapon);
        }
    }
}
