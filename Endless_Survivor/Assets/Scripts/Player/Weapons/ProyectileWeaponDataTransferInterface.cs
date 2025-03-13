using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProyectileWeaponDataTransferInterface : ShootingWeaponDataTransferInterface
{
    [SerializeField] ProyectileData _defaultBulletData;
    [SerializeField] float _proyectileSpeed;
    [SerializeField] float _proyectileSpread;
    public override void TransferData(GameObject weaponObject, WeaponData weaponData)
    {
        ProyectileWeapon proyectileWeaponComponent = weaponObject.AddComponent<ProyectileWeapon>();
        proyectileWeaponComponent.ProyectileData = new ProyectileData(_defaultBulletData);
        proyectileWeaponComponent.ProyectileSpeed = _proyectileSpeed;
        proyectileWeaponComponent.ProyectileSpread = _proyectileSpread;
        base.TransferData(weaponObject, weaponData);
    }
}
