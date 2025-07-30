using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Passive Item Pickup", menuName = "ScriptableObjects/Pickups/PassiveItem", order = 2)]
public class PassiveItemPickupData : PickupData
{
    [SerializeField] CustomFlags.IPassiveItemPool _itemPools;
    static readonly string _itemVariableKey = "pickupItem";
    [SerializeField] CustomAnimation _itemBaseIdle;
    [SerializeField] CustomAnimation _itemBasePickup;
    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        var unlockedPassiveItems = UnlockmentsManager.UnlockedPassiveItems;
        var availableItems = unlockedPassiveItems.Where(x => (x.ItemPools & _itemPools) != CustomFlags.IPassiveItemPool.None).ToList();
        PassiveItemData pickupPassiveItem = availableItems[Random.Range(0, availableItems.Count)];//get a random passive
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
