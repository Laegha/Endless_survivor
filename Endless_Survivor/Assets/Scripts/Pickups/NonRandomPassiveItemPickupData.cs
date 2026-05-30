using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New Non Random Passive Item Pickup", menuName = "ScriptableObjects/Pickups/NonRandomPassiveItem", order = 11)]
public class NonRandomPassiveItemPickupData : PickupData
{
    [SerializeField] PassiveItemData _item;
    static readonly string _itemVariableKey = "pickupNonRandomItem";
    public override void TransferData(PickupControl pickupControl)
    {
        base.TransferData(pickupControl);
        pickupControl.Pickup.AddVariable(_itemVariableKey, _item);
        pickupControl.Renderer.sprite = _item.ItemSprite;
    }
    public override void PickUp(PickupControl pickupControl)
    {
        base.PickUp(pickupControl);
        var itemElementInfo = pickupControl.Pickup.GetVariable<PassiveItemData>(_itemVariableKey);
        GameUIManager.uiManager.PassiveItemPickupMenu.DisplayMenu(itemElementInfo, false);
    }
}
