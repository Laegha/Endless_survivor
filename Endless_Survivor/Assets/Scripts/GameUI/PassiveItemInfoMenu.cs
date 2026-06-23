using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PassiveItemInfoMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] ScalableToTargetImage _itemImage;
    [SerializeField] TextMeshProUGUI _itemDescript;

    public void UpdateInfo(PassiveItemData newShowingItem)
    {
        _itemName.text = newShowingItem.ItemName;
        _itemImage.ChangeImageSprite(newShowingItem.ItemSprite);
        _itemDescript.text = newShowingItem.ItemDescript;
    }
}
