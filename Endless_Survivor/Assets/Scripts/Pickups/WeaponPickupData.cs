using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Weapon Pickup", menuName = "ScriptableObjects/Pickups/Weapon", order = 2)]
public class WeaponPickupData : PickupData
{
    static readonly string _weaponVariableKey = "pickupWeapon";
    [SerializeField] WeaponFlags.IWeaponPool _droppedWeaponPool;
    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);

        var resultWeaponData = RandomWeaponGetter.GetWeapon(true, _droppedWeaponPool);
        pickupControl.Pickup.AddVariable(_weaponVariableKey, resultWeaponData);
        CustomAnimation weponIdle = resultWeaponData.Animations.Where(animation => animation.AnimationName == "Idle").ToArray()[0];

        PickupControl control = pickupControl.GetComponent<PickupControl>();
        control.Animator.AddAnimations(new List<CustomAnimation> { weponIdle });
        control.Animator.ChangeAnim("Idle");
    }
    public override void PickUp(PickupControl pickupControl)
    {
        base.PickUp(pickupControl);
        GameUIManager.uiManager.WeaponPickupMenu.DisplayMenu(pickupControl.Pickup.GetVariable<WeaponData>(_weaponVariableKey));
    }
}
