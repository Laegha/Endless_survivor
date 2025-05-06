    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemPickupData : PickupData
{
    static readonly int _regularWeaponSpawnChance = 10;
    static readonly string _itemVariableKey = "pickupItem";
    [SerializeField] CustomAnimation _itemBaseIdle;
    [SerializeField] CustomAnimation _itemBasePickup;
    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        PassiveItemData pickupPassiveItem = null;//get a random passive
        pickupControl.Pickup.AddVariable(_itemVariableKey, pickupPassiveItem);
        pickupControl.Animator.AddAnimations(new List<CustomAnimation> { _itemBaseIdle, _itemBasePickup });
        pickupControl.Animator.ChangeAnim(_itemBaseIdle.AnimationName);
        //pickupControl.Renderer.sprite 
    }
    public override void PickUp(PickupControl pickupControl)
    {
        base.PickUp(pickupControl);
        pickupControl.Animator.ChangeAnim(_itemBasePickup.AnimationName);
        PlayerControl.pc.PassiveItemManager.AddPassiveItem(pickupControl.Pickup.GetVariable<PassiveItemData>(_itemVariableKey));
        Destroy(pickupControl.gameObject, _itemBasePickup.AnimDuration);
    }
}
