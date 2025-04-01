using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    float _shootCooldown;
    bool _canShoot;
    WeaponStats _weaponStats;
    WeaponControl _weaponControl;
    WeaponData _weaponData;
    PlayerControl _playerControl;

    public WeaponStats WeaponStats {  get { return _weaponStats; } set { _weaponStats = value; } }
    public WeaponControl WeaponControl { get { return _weaponControl; } }
    public WeaponData WeaponData { get { return _weaponData; } set { _weaponData = value; } }
    public PlayerControl PlayerControl { get { return _playerControl; } set { _playerControl = value; } }

    public virtual void Start()
    {
        _weaponControl = GetComponent<WeaponControl>();
        _weaponControl.WeaponAnimator.ChangeAnim("Idle");
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

    public void TryAttack()
    {
        if (!_canShoot)
            return;

        _canShoot = false;
        _shootCooldown = 1f / WeaponStats.AttackSpeed + GameManager.gm.selectedCharacter.PlayerStats.AttackSpeed;
        Attack();
    }

    public virtual void Attack()
    {
        print("Attacking");
    }
}
