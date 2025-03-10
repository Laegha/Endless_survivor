using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RayWeaponDataTransferInterface : ShootingWeaponDataTransferInterface
{
    [SerializeField] Material _rayMaterial;
    [SerializeField] float _rayStartWidth;
    [SerializeField] float _rayEndWidth;
    public override void TransferData(GameObject weaponObject, WeaponData weaponData)
    {
        RayWeapon sniperComponent = weaponObject.AddComponent<RayWeapon>();
        LineRenderer ln = new GameObject().AddComponent<LineRenderer>();
        ln.transform.name = "RayRenderer";
        ln.material = _rayMaterial;
        ln.startWidth = _rayStartWidth;
        ln.endWidth = _rayEndWidth;
        ln.transform.SetParent(weaponObject.transform, false);
        sniperComponent.LineRenderer = ln;

        base.TransferData(weaponObject, weaponData);
        ln.SetPosition(0, sniperComponent.FirePoint.position);
    }
}
