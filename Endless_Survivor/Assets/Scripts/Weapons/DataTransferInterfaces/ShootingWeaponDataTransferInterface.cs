using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingWeaponDataTransferInterface : WeaponDataTransferInterface
{
    [Header("Fire point position in pixels (from center of sprite, set in the sprite editor")]
    [SerializeField] Vector2 _firePointPosition;
    public override void TransferData(GameObject weaponObject, WeaponData weaponData, WeaponStats weaponStats)
    {
        Transform firePoint = new GameObject().transform;
        firePoint.transform.SetParent(weaponObject.transform, false);
        firePoint.name = "FirePoint";
        firePoint.localPosition = _firePointPosition / 32;
        firePoint.localRotation = Quaternion.identity;
        weaponObject.GetComponent<ShootingWeapon>().FirePoint = firePoint;
        
        base.TransferData(weaponObject, weaponData, weaponStats);
    }
}
