using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    float _shootCooldown;
    bool _canShoot;
    bool _inRange;
    WeaponStats _weaponStats;
    WeaponControl _weaponControl;
    WeaponData _weaponData;
    PlayerControl _playerControl;
    Action<Attack> _initializeAttack;
    AttackEffectsHolder _weaponAttackEffects = new();
    static readonly float _onLevelUpParticleDuration = 1.5f;

    public WeaponStats WeaponStats {  get { return _weaponStats; } set { _weaponStats = value; } }
    public WeaponControl WeaponControl { get { return _weaponControl; } }
    public WeaponData WeaponData { get { return _weaponData; } set { _weaponData = value; } }
    public PlayerControl PlayerControl { get { return _playerControl; } set { _playerControl = value; } }
    public bool InRange { set  { _inRange = value; } }
    public Action<Attack> InitializeAttack { get { return _initializeAttack; } set { _initializeAttack = value; }  }
    public AttackEffectsHolder WeaponAttackEffects { get { return _weaponAttackEffects; } set { _weaponAttackEffects = value; } }

    public virtual void Start()
    {
        _weaponControl = GetComponent<WeaponControl>();
        _weaponControl.WeaponAnimator.ChangeAnim("Idle");
        var attackAnimation = _weaponControl.WeaponAnimator.Animations.Find(x => x.AnimationName == "Attack");
        var attackEndEvent = new AnimationEvent();
        attackEndEvent.frameIndex = attackAnimation.Frames.Length-1;
        attackEndEvent.frameAction = AttackAnimEnd;
        attackAnimation.Events.Add(attackEndEvent);

        _initializeAttack += SetWeaponOnAttack;
    }

    public virtual void Update()
    {
        if (_shootCooldown > 0)
        {
            _shootCooldown -= Time.deltaTime;
            return;
        }
        if(!_canShoot)
            _canShoot = true;
    }

    public virtual void ChangeAttackGfx(AttackGfxInterface gfxInterface) { }

    public void TryAttack()
    {
        if (!_canShoot)
            return;

        _canShoot = false;
        _shootCooldown = 1f / (WeaponStats.AttackSpeed + GameManager.gm.selectedCharacter.PlayerStats.AttackSpeed);
        Attack();
    }
    void AttackAnimEnd()
    {
        if(!_canShoot || !_inRange)
        {
            _weaponControl.WeaponAnimator.ChangeAnim("Idle");
        }
    }

    public virtual void Attack()
    {
        print("Attacking");
    }

    void SetWeaponOnAttack(Attack attack)
    {
        attack.ParentWeapon = this;
    }

    public void InducedLevelUp()
    {
        _weaponStats.InducedLevelUp(_weaponData.StatsIncreaseScale);
        //spawn a little lvl up particle or smth
        ParticleConfig levelUpParticles = new(_weaponControl.OnLevelUpParticle, transform.position, Quaternion.identity, _onLevelUpParticleDuration);
        ParticleManager.pm.SpawnParticles(levelUpParticles);
    }
}
