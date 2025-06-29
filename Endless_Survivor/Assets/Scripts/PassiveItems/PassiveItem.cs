using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItem
{
    Sprite _itemSprite;
    PassiveItemBehaviourManager _behaviourManager;
    public Sprite ItemSprite {  set { _itemSprite = value; } }
    public PassiveItemBehaviourManager BehaviourManager { get { return _behaviourManager; } }
    public PassiveItem()
    {
        _behaviourManager = new();
    }
}
