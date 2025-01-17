using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    float _shootCooldown;
    bool _canShoot;
    [SerializeField] WeaponData _data;
    WeaponStats _weaponStats;

    public WeaponData Data {  get { return _data; } }

    private void Start()
    {
        _weaponStats = new WeaponStats(_data.WeaponStats);
    }

    private void Update()
    {
        if (_shootCooldown > 0)
        {
            _shootCooldown -= Time.deltaTime;
            return;
        }
        if(!_canShoot)
            _canShoot = true;
    }


    public virtual void Attack()
    {
        if (!_canShoot)
            return;

        _canShoot = false;
        _shootCooldown = 1/(Data.WeaponStats.AttackSpeed + GameManager.gm.selectedCharacter.PlayerStats.AttackSpeed);
    }
}
