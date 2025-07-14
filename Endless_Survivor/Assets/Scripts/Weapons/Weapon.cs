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

    public WeaponStats WeaponStats {  get { return _weaponStats; } set { _weaponStats = value; } }
    public WeaponControl WeaponControl { get { return _weaponControl; } }
    public WeaponData WeaponData { get { return _weaponData; } set { _weaponData = value; } }
    public PlayerControl PlayerControl { get { return _playerControl; } set { _playerControl = value; } }
    public bool InRange { set  { _inRange = value; } }

    public virtual void Start()
    {
        _weaponControl = GetComponent<WeaponControl>();
        _weaponControl.WeaponAnimator.ChangeAnim("Idle");
        var attackAnimation = _weaponControl.WeaponAnimator.Animations.Find(x => x.AnimationName == "Attack");
        var attackEndEvent = new AnimationEvent();
        attackEndEvent.frameIndex = attackAnimation.Frames.Length-1;
        attackEndEvent.frameAction = AttackAnimEnd;
        attackAnimation.Events.Add(attackEndEvent);
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
}
