using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponAttackManager : MonoBehaviour
{
    WeaponAttackController _currAttackController;
    List<WeaponAttackChangeCondition> _attackConditions = new();
    WeaponAttackController _defaultAttack;
    List<WeaponAttackController> _attackControllers = new();

    float _attackCooldown;
    bool _inCooldown;
    bool _prevAttackFinished = true;
    bool _inRange;
    bool _reduceCooldown = true;
    WeaponStats _weaponStats;
    WeaponControl _weaponControl;
    WeaponData _weaponData;
    static readonly float _onLevelUpParticleDuration = 1.5f;

    public bool InRange { get { return _inRange; } set { _inRange = value; } }
    public WeaponData WeaponData { get { return _weaponData; } }
    public WeaponStats WeaponStats { get { return _weaponStats; } set { _weaponStats = value; } }
    public List<WeaponAttackController> AttackControllers { get { return _attackControllers; } }
    public WeaponAttackController CurrAttackController { get { return _currAttackController; } }

    public void Initiate(List<WeaponAttackChangeCondition> attackConditions, WeaponStats stats, WeaponData data)
    {
        foreach(var condition in attackConditions)
        {
            WeaponAttackChangeCondition newCondition = Activator.CreateInstance(condition.GetType()).ConvertTo<WeaponAttackChangeCondition>();
            newCondition.CopyData(this, condition);
            _attackConditions.Add(newCondition);
        }
        _weaponStats = new(stats);
        _weaponData = data;
        _weaponControl = GetComponent<WeaponControl>();

        _weaponControl.WeaponAnimator.AddAnimations(new(){ data.IdleAnim });
        if (data.RandomIdleAnimations.Count > 0)
        {
            var idleAnimator = _weaponControl.AddComponent<RandomIdleAnimator>();
            idleAnimator.SetData(_weaponControl.WeaponAnimator, data.RandomIdleAnimChance, data.RandomIdleAnimTime, data.RandomIdleAnimations);
        }
        _weaponControl.WeaponAnimator.ChangeAnim(data.IdleAnim.AnimationName);
        WeaponAttackController defaultAttackController = Activator.CreateInstance(data.DefaultAttack.GetType()) as WeaponAttackController;
        defaultAttackController.Initialize(_weaponControl, data.DefaultAttack);
        _defaultAttack = defaultAttackController;
        foreach(var attackController in data.WeaponAttacks)
        {
            var instantiatedController = Activator.CreateInstance(attackController.GetType()) as WeaponAttackController;
            instantiatedController.Initialize(_weaponControl, attackController);
            _attackControllers.Add(instantiatedController);
        }
    }

    public void ChangeAttackController(WeaponAttackController newAttackController)
    {
        _currAttackController?.End();
        _currAttackController = newAttackController;
    }
    void GetNewAttackController()
    {
        var newAttackController = _defaultAttack;
        foreach (var condition in _attackConditions)
        {
            if(!condition.IsConditionMet())
                continue;
            newAttackController = condition.GetAttackController();
            break;
        }
        if(newAttackController != _currAttackController)
            ChangeAttackController(newAttackController);
    }

    private void Update()
    {
        _currAttackController?.Update(); 
        if (!_reduceCooldown)
            return;

        if (_attackCooldown > 0)
        {
            _attackCooldown -= Time.deltaTime;
            return;
        }
        if (!_inCooldown)
            _inCooldown = true;
    }
    public void TryAttack()
    {
        if (!_inCooldown || !_prevAttackFinished)
            return;

        _inCooldown = false;
        _prevAttackFinished = false;
        _attackCooldown = 1f / (_weaponStats.AttackSpeed);
        GetNewAttackController();
        _currAttackController.StartAttack();
        //_weaponControl.WeaponAnimator.ChangeAnim(_currAttackController.AnimationName);
    }
    public void InducedLevelUp()
    {
        _weaponStats.InducedLevelUp(_weaponData.StatsIncreaseScale);
        //spawn a little lvl up particle or smth
        ParticleConfig levelUpParticles = new(_weaponControl.OnLevelUpParticle, transform.position, Quaternion.identity, _onLevelUpParticleDuration);
        ParticleManager.pm.SpawnParticles(levelUpParticles);
    }
    public void OverrideAttackCooldown(float newCooldown) => _attackCooldown = newCooldown;
    public void PauseAttackCooldown()
    {
        _reduceCooldown = false;
    }
    public void UnPauseAttackCooldown()
    {
        _reduceCooldown = true;
    }
    public void FinishAttack() => _prevAttackFinished = true;
}
