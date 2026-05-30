using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Non Random Weapon Pickup", menuName = "ScriptableObjects/Pickups/NonRandomWeapon", order = 10)]
public class NonRandomWeaponPickupData : PickupData
{
    static readonly string _weaponVariableKey = "pickupNonRandomWeapon";
    [SerializeField] WeaponData _droppedWeapon;
    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        Transfer(pickupControl);
    }
    void Transfer(PickupControl pickupControl)
    {
        pickupControl.Pickup.AddVariable(_weaponVariableKey, _droppedWeapon);
        CustomAnimation weponIdle = _droppedWeapon.IdleAnim;

        PickupControl control = pickupControl.GetComponent<PickupControl>();
        control.Animator.AddAnimations(new List<CustomAnimation> { weponIdle });
        control.Animator.ChangeAnim("Idle");
    }
    public override void PickUp(PickupControl pickupControl)
    {
        base.PickUp(pickupControl);
        var weaponElementInfo = pickupControl.Pickup.GetVariable<WeaponData>(_weaponVariableKey);
        GameUIManager.uiManager.WeaponPickupMenu.DisplayMenu(weaponElementInfo, false);
    }
}
