using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveItemPickupMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuGfx;
    [SerializeField] Image _itemImage;
    [SerializeField] RectTransform _itemImageTargetSize;
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] TextMeshProUGUI _itemDescript;
    PassiveItemData _currPickingItem;

    public void DisplayMenu(PassiveItemData pickingItem)
    {
        _menuGfx.SetActive(true);
        GameUIManager.uiManager.MenuDisplayed();
        _currPickingItem = pickingItem;
        _itemImage.sprite = pickingItem.ItemSprite;
        Utility.ScaleImageToFitTarget(_itemImage.rectTransform, pickingItem.ItemSprite, _itemImageTargetSize.sizeDelta);
        _itemName.text = pickingItem.ItemName;
        _itemDescript.text = pickingItem.ItemDescript;

    }
    public void TakeItem()
    {
        PlayerControl.pc.PassiveItemManager.AddPassiveItem(_currPickingItem);
        _menuGfx.SetActive(false);
        GameUIManager.uiManager.MenuHid();
    }
    public void DiscardItem()
    {
        _menuGfx.SetActive(false);
        GameUIManager.uiManager.MenuHid();
        PlayerControl.pc.WeaponManager.LevelUpWeapons();
    }
}
