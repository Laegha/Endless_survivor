using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SniperDataTransferInterface : WeaponDataTransferInterface
{
    [SerializeField] Material _rayMaterial;
    [SerializeField] float _rayStartWidth;
    [SerializeField] float _rayEndWidth;
    public override void TransferData(GameObject weaponObject, WeaponData weaponData)
    {
        base.TransferData(weaponObject, weaponData);
        Sniper sniperComponent = weaponObject.AddComponent<Sniper>();
        LineRenderer ln = MonoBehaviour.Instantiate(new GameObject()).AddComponent<LineRenderer>();
        ln.material = _rayMaterial;
        ln.startWidth = _rayStartWidth;
        ln.endWidth = _rayEndWidth;
        ln.transform.SetParent(weaponObject.transform, false);
        sniperComponent.LineRenderer = ln;

    }
}
