using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RayWeaponDataTransferInterface : ShootingWeaponDataTransferInterface
{
    public static bool isUsable => true;
    [SerializeField] RayData _rayData;
    public override void TransferData(GameObject weaponObject, WeaponData weaponData, WeaponStats weaponStats)
    {
        RayWeapon sniperComponent = weaponObject.AddComponent<RayWeapon>();
        sniperComponent.RayData = new RayData(_rayData);
        LineRenderer ln = new GameObject().AddComponent<LineRenderer>();
        ln.transform.name = "RayRenderer";
        ln.material = _rayData.RayMaterial;
        ln.startWidth = _rayData.RayStartWidth;
        ln.endWidth = _rayData.RayEndWidth;
        ln.transform.SetParent(weaponObject.transform, false);

        base.TransferData(weaponObject, weaponData, weaponStats);
        ln.SetPosition(0, sniperComponent.FirePoint.position);
    }
}
