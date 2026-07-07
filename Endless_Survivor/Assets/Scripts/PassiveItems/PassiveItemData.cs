using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New PassiveItem", menuName = "ScriptableObjects/Passive Item", order = 2)]
public class PassiveItemData : ScriptableObject
{
    [SerializeField] string _itemName;
    [SerializeField] string _itemDescript;
    [SerializeField] Sprite _itemSprite;
    [SerializeField] List<PassiveItemOverride> _itemOverrides;
    [SerializeReference] List<PassiveItemBehaviour> _itemBehaviours = new List<PassiveItemBehaviour>();
    [SerializeField] CustomFlags.IPassiveItemPool _itemPools;

    public string ItemName { get { return _itemName; } }
    public string ItemDescript {  get { return _itemDescript; } }
    public Sprite ItemSprite { get { return _itemSprite; } }
    public List<PassiveItemOverride> ItemOverrides { get { return _itemOverrides; } }
    public List<PassiveItemBehaviour> ItemBehaviours { get { return _itemBehaviours; } }
    public CustomFlags.IPassiveItemPool ItemPools { get { return _itemPools; } }

    public void TransferData(PassiveItem item)
    {
        item.ItemData = this;
        item.ItemSprite = _itemSprite;
        item.BehaviourManager.CopyBehaviours(_itemBehaviours);
    }
}
