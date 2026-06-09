using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveItemsOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<PassiveItemData> _addingItems;
    [SerializeField] bool _removeItemsAfterTime;
    [SerializeField] bool _removeItemsIfThisIsRemoved;
    [SerializeField] float _timeToRemoveItems;

    List<PassiveItem> _addedItems;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var giveItemsOriginal = original as GiveItemsOnActivateItemBehaviour;
        _addingItems = giveItemsOriginal._addingItems;
        _removeItemsAfterTime = giveItemsOriginal._removeItemsAfterTime;
        _removeItemsIfThisIsRemoved = giveItemsOriginal._removeItemsIfThisIsRemoved;
        _timeToRemoveItems = giveItemsOriginal._timeToRemoveItems;
    }

    public override void Activate()
    {
        base.Activate();
        AddItems();
    }

    void AddItems()
    {
        foreach (var itemData in _addingItems)
        {
            var addedItem = PlayerControl.pc.PassiveItemManager.AddPassiveItem(itemData);
            _addedItems.Add(addedItem);
        }
        if (_removeItemsAfterTime)
            GameManager.gm.DelayAction(_timeToRemoveItems, RemoveAnIterationOfItems, () => BehaviourManager == null);
    }
    void RemoveAnIterationOfItems()
    {
        List<PassiveItem> addedItemsCopy = new(_addedItems);
        for(int i = 0; i < _addingItems.Count; i++)
        {
            PlayerControl.pc.PassiveItemManager.RemovePassiveItem(addedItemsCopy[i]);
            _addedItems.RemoveAt(i);
        }
    }
    void RemoveAllItems()
    {
        foreach(var item in _addedItems)
        {
            PlayerControl.pc.PassiveItemManager.RemovePassiveItem(item);
        }
    }
    public override void RemoveBehaviour()
    {
        if(_removeItemsIfThisIsRemoved)
            RemoveAllItems();
    }

}
