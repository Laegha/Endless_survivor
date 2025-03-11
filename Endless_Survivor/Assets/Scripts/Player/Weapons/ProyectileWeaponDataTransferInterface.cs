using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProyectileWeaponDataTransferInterface : ShootingWeaponDataTransferInterface
{
    [SerializeField] BulletData _defaultBulletData;
    public override void TransferData(GameObject weaponObject, WeaponData weaponData)
    {
        ProyectileWeapon proyectileWeaponComponent = weaponObject.AddComponent<ProyectileWeapon>();
        proyectileWeaponComponent.BulletData = new BulletData(_defaultBulletData);
        base.TransferData(weaponObject, weaponData);
    }
}
