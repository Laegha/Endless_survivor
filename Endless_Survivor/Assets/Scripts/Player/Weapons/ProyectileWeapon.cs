using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileWeapon : ShootingWeapon
{
    [SerializeField] GameObject _bulletPrefab;
    public override void Attack()
    {
        base.Attack();
        WeaponControl.WeaponAnimator.ChangeAnim("Attack");
        Transform bullet = Instantiate(_bulletPrefab).transform;
        bullet.position = FirePoint.position;
        bullet.rotation = FirePoint.rotation;
    }
}
