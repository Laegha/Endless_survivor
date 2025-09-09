using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeaponDataTransferInterface : WeaponDataTransferInterface
{
    public static bool isUsable => true;
    [SerializeField] MeleeData _meleeData;
    public override void TransferData(GameObject weaponObject, WeaponData weaponData, WeaponStats weaponStats)
    {
        var meleeWeaponComponent = weaponObject.AddComponent<MeleeWeapon>();
        meleeWeaponComponent.MeleeData = new(_meleeData);
        base.TransferData(weaponObject, weaponData, weaponStats);
    }
}
