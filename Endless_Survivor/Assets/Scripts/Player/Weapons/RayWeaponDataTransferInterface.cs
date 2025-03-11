using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RayWeaponDataTransferInterface : ShootingWeaponDataTransferInterface
{
    [SerializeField] RayData _rayData;
    public override void TransferData(GameObject weaponObject, WeaponData weaponData)
    {
        RayWeapon sniperComponent = weaponObject.AddComponent<RayWeapon>();
        LineRenderer ln = new GameObject().AddComponent<LineRenderer>();
        ln.transform.name = "RayRenderer";
        ln.material = _rayData.RayMaterial;
        ln.startWidth = _rayData.RayStartWidth;
        ln.endWidth = _rayData.RayEndWidth;
        ln.transform.SetParent(weaponObject.transform, false);
        sniperComponent.LineRenderer = ln;

        base.TransferData(weaponObject, weaponData);
        ln.SetPosition(0, sniperComponent.FirePoint.position);
    }
}
