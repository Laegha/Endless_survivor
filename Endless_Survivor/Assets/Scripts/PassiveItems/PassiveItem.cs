using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem
{
    PassiveItemData _itemData;
    Sprite _itemSprite;
    PassiveItemBehaviourManager _behaviourManager;
    public PassiveItemData ItemData { get { return _itemData; } set { _itemData = value; } }
    public Sprite ItemSprite {  set { _itemSprite = value; } }
    public PassiveItemBehaviourManager BehaviourManager { get { return _behaviourManager; } }
    public PassiveItem()
    {
        _behaviourManager = new();
    }
}
