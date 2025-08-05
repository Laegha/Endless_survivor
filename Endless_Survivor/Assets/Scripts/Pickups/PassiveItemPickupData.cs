using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static CustomFlags;

[CreateAssetMenu(fileName = "New Passive Item Pickup", menuName = "ScriptableObjects/Pickups/PassiveItem", order = 2)]
public class PassiveItemPickupData : PickupData
{
    [SerializeField] CustomFlags.IPassiveItemPool _itemPools;
    static readonly string _itemVariableKey = "pickupItem";
    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        var availableItems = UnlockmentsManager.UnlockedPassiveItems;
        if (_itemPools != CustomFlags.IPassiveItemPool.None)
            availableItems = availableItems.Where(x => (x.ItemPools & _itemPools) != CustomFlags.IPassiveItemPool.None).ToList();
        
        PassiveItemData pickupPassiveItem = availableItems[Random.Range(0, availableItems.Count)];//get a random passive
        pickupControl.Pickup.AddVariable(_itemVariableKey, pickupPassiveItem);
        pickupControl.Renderer.sprite = pickupPassiveItem.ItemSprite;
    }
    public override void PickUp(PickupControl pickupControl)
    {
        base.PickUp(pickupControl);
        GameUIManager.uiManager.PassiveItemPickupMenu.DisplayMenu(pickupControl.Pickup.GetVariable<PassiveItemData>(_itemVariableKey));
    }
}
