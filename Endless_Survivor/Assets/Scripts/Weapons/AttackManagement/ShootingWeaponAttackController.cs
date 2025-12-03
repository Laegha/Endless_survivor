using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ShootingWeaponAttackController : WeaponAttackController
{
    new public static bool isUsable => false;
    [Header("Fire point position in pixels (from center of sprite, set in the sprite editor")]
    [SerializeField] Vector2 _firePointPosition;
    Transform _firePoint;
    bool _isYFlipped = false;
    bool _isXFlipped = false;
    Vector2 _firePointPos;
    public Transform FirePoint { get { return _firePoint; } set { _firePoint = value; } }

    public override void Initialize(WeaponControl weaponControl, WeaponAttackController original)
    {
        base.Initialize(weaponControl, original);
        ShootingWeaponAttackController shootingOriginal = original as ShootingWeaponAttackController;
        _firePointPosition = shootingOriginal._firePointPosition;
        _firePoint = new GameObject().transform;
        _firePoint.transform.SetParent(weaponControl.transform, false);
        _firePoint.name = AttackId + " FirePoint";
        _firePointPos = _firePointPosition / 32;
        _firePoint.localPosition = _firePointPos;
        _firePoint.localRotation = Quaternion.identity;
    }
    public override void Update()
    {
        base.Update();
        if(_isYFlipped != WeaponControl.Gfx.flipY || _isXFlipped != WeaponControl.Gfx.flipX)
        {
            _isYFlipped = WeaponControl.Gfx.flipY;
            _isXFlipped = WeaponControl.Gfx.flipX;
            _firePoint.localPosition = new Vector2(_isXFlipped ? -_firePointPos.x : _firePointPos.x, _isYFlipped ? -_firePointPos.y : _firePointPos.y);
        }
    }
}
