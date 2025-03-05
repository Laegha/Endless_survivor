using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    float _shootCooldown;
    bool _canShoot;
    WeaponStats _weaponStats;
    CustomAnimator _customAnimator;

    public WeaponStats WeaponStats {  get { return _weaponStats; } set { _weaponStats = value; } }
    public CustomAnimator WeaponAnimator { get { return _customAnimator; } }

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

    public void TryAttack()
    {
        if (!_canShoot)
            return;

        _canShoot = false;
        _shootCooldown = 1 / (WeaponStats.AttackSpeed + GameManager.gm.selectedCharacter.PlayerStats.AttackSpeed);
        Attack();
    }

    public virtual void Attack()
    {
        print("Attacking");
    }
}
