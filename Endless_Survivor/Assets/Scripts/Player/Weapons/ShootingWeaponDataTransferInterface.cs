using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingWeaponDataTransferInterface : WeaponDataTransferInterface
{
    [SerializeField] Vector2 _firePointPosition;
    public override void TransferData(GameObject weaponObject, WeaponData weaponData)
    {
        base.TransferData(weaponObject, weaponData);
        Transform firePoint = GameObject.Instantiate(new GameObject(), weaponObject.transform).transform;
        firePoint.localPosition = _firePointPosition;
        weaponObject.GetComponent<ShootingWeapon>().FirePoint = firePoint;
        
    }
}
