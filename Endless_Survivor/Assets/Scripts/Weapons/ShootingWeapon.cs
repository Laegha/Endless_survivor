using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingWeapon : Weapon
{
    Transform _firePoint;
    bool _isYFlipped = false;
    bool _isXFlipped = false;
    Vector2 _firePointPos;
    public Transform FirePoint { get { return _firePoint; } set { _firePoint = value; } }

    public override void Start()
    {
        base.Start();
        _firePointPos = _firePoint.localPosition;
        //WeaponControl.WeaponAim.ChangeDirectionBase(_firePoint);
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
