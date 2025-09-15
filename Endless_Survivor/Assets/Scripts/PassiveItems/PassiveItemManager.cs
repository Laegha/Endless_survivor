using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    List<PassiveItem> _passiveItems = new();

    public void AddPassiveItem(PassiveItemData itemData)
    {
        PassiveItem addedItem = new PassiveItem();
        itemData.TransferData(addedItem);
        addedItem.BehaviourManager.onPicked?.Invoke();
        _passiveItems.Add(addedItem);
    }
    private void Update()
    {
        _passiveItems.ForEach(item => item.BehaviourManager.onUpdate?.Invoke());
    }
    public void WeaponAttack(Weapon weapon)
    {
        _passiveItems.ForEach(item => item.BehaviourManager.onAttack?.Invoke(weapon));

    }

}
