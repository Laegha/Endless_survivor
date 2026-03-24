using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Pickup", menuName = "ScriptableObjects/Pickups/Weapon", order = 2)]
public class WeaponPickupData : PickupData
{
    static readonly string _weaponVariableKey = "pickupWeapon";
    [SerializeField] CustomFlags.IWeaponPool _droppedWeaponPool;
    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        TransferAsync(pickupControl);
    }
    void TransferAsync(PickupControl pickupControl)
    {
        var resultWeaponData = RandomWeaponGetter.GetWeapon(true, _droppedWeaponPool);
        pickupControl.Pickup.AddVariable(_weaponVariableKey, resultWeaponData);
        CustomAnimation weponIdle = resultWeaponData.element.IdleAnim;

        PickupControl control = pickupControl.GetComponent<PickupControl>();
        control.Animator.AddAnimations(new List<CustomAnimation> { weponIdle });
        control.Animator.ChangeAnim("Idle");
    }
    public override void PickUp(PickupControl pickupControl)
    {
        base.PickUp(pickupControl);
        var weaponElementInfo = pickupControl.Pickup.GetVariable<ElementIsNewInfo<WeaponData>>(_weaponVariableKey);
        GameUIManager.uiManager.WeaponPickupMenu.DisplayMenu(weaponElementInfo.element, weaponElementInfo.isNew);
    }
}
