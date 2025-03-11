using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProyectileWeapon : ShootingWeapon
{
    [SerializeField] GameObject _bulletPrefab;
    BulletData _bulletData;
    public override void Attack()
    {
        base.Attack();
        WeaponControl.WeaponAnimator.ChangeAnim("Attack");
        Bullet bullet = Instantiate(_bulletPrefab).GetComponent<Bullet>();
        bullet.transform.position = FirePoint.position;
        bullet.transform.rotation = FirePoint.rotation;
        bullet.SpriteRenderer.sprite = _bulletData.BulletSprite;
        bullet.Collider.size = _bulletData.ColliderSize;
    }
}
