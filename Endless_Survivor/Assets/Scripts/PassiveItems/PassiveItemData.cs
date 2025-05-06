using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PassiveItem", menuName = "ScriptableObjects/Passive Item", order = 2)]
public class PassiveItemData : ScriptableObject
{
    [SerializeField] Sprite _itemSprite;
    [SerializeReference] List<PassiveItemBehaviour> _itemBehaviours = new List<PassiveItemBehaviour>();
    public List<PassiveItemBehaviour> ItemBehaviours { get { return _itemBehaviours; } }

    public void TransferData(PassiveItem item)
    {
        item.ItemSprite = _itemSprite;
        item.BehaviourManager.CopyBehaviours(_itemBehaviours);
    }
}
