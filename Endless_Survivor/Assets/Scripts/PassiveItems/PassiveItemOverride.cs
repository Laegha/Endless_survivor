using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PassiveItemOverride
{
    [SerializeField] PassiveItemData _overridenItem;
    [SerializeField] bool _isItemRemovedPerm;

    public PassiveItemData OverridenItem { get { return _overridenItem; } }
    public bool IsItemRemovedPerm { get { return _isItemRemovedPerm; } }
}
