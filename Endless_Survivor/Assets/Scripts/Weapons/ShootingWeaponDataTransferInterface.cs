using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingWeaponDataTransferInterface : WeaponDataTransferInterface
{
    [SerializeField] Vector2 _firePointPosition;
    public override void TransferData(GameObject weaponObject, WeaponData weaponData)
    {
        Transform firePoint = new GameObject().transform;
        firePoint.transform.SetParent(weaponObject.transform, false);
        firePoint.name = "FirePoint";
        firePoint.localPosition = _firePointPosition;
        firePoint.localRotation = Quaternion.identity;
        weaponObject.GetComponent<ShootingWeapon>().FirePoint = firePoint;
        
        base.TransferData(weaponObject, weaponData);
    }
}
