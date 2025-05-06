using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    List<PassiveItem> _passiveItems;

    public void AddPassiveItem(PassiveItemData itemData)
    {
        PassiveItem addedItem = new PassiveItem();
        itemData.TransferData(addedItem);
        addedItem.BehaviourManager.onPicked();
        _passiveItems.Add(addedItem);
    }
    private void Update()
    {
        _passiveItems.ForEach(item => item.BehaviourManager.onUpdate());
    }

}
