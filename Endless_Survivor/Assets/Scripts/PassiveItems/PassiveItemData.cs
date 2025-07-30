using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PassiveItem", menuName = "ScriptableObjects/Passive Item", order = 2)]
public class PassiveItemData : ScriptableObject
{
    [SerializeField] Sprite _itemSprite;
    [SerializeReference] List<PassiveItemBehaviour> _itemBehaviours = new List<PassiveItemBehaviour>();
    [SerializeField] CustomFlags.IPassiveItemPool _itemPools;

    public List<PassiveItemBehaviour> ItemBehaviours { get { return _itemBehaviours; } }
    public CustomFlags.IPassiveItemPool ItemPools { get { return _itemPools; } }

    public void TransferData(PassiveItem item)
    {
        item.ItemSprite = _itemSprite;
        item.BehaviourManager.CopyBehaviours(_itemBehaviours);
    }
}
