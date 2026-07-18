using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PassiveItemPickupMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuGfx;
    [SerializeField] GameObject _newItemIndicator;
    [SerializeField] Image _itemImage;
    [SerializeField] RectTransform _itemImageTargetSize;
    [SerializeField] TextMeshProUGUI _itemName;
    [SerializeField] TextMeshProUGUI _itemDescript;
    [SerializeField] TextMeshProUGUI _rerollsLeftDisplay;
    [SerializeField] InformationButton _itemInfoButton;
    [SerializeField] GameObject _cantPickWeaponButton;
    PassiveItemData _currPickingItem;
    GameObject _currNewIndicator;
    Action _onMenuOpen;
    public Action OnMenuOpen { get { return _onMenuOpen; } set { _onMenuOpen = value; } }
    public void DisplayMenu(PassiveItemData pickingItem, bool isNew)
    {
        _menuGfx.SetActive(true);
        GameUIManager.uiManager.MenuDisplayed();
        _onMenuOpen?.Invoke();
        SetItemMenu(pickingItem, isNew);

    }
    public void TakeItem()
    {
        PlayerControl.pc.PassiveItemManager.AddPassiveItem(_currPickingItem);
        if(_currPickingItem != null)
        {
            UnlockmentsManager.SetNotNewPassiveItem(_currPickingItem);
            Destroy(_currNewIndicator);
        }
        _menuGfx.SetActive(false);
        GameUIManager.uiManager.MenuHid();
    }
    public void DiscardItem()
    {
        _menuGfx.SetActive(false);
        GameUIManager.uiManager.MenuHid();
        PlayerControl.pc.WeaponManager.LevelUpWeapons();
    }
    public void RerollItem()
    {
        if (RerollManager.rm.RerollsLeft <= 0)
            return;

        RerollManager.rm.UseReroll();
        var availableItems = GameManager.gm.UnlockedElementHelper.UnlockedPassiveItems;
        var newItem = availableItems[UnityEngine.Random.Range(0, availableItems.Count)];//get a random passive
        while(newItem.element == _currPickingItem)
            newItem = availableItems[UnityEngine.Random.Range(0, availableItems.Count)];//get a random passive
        SetItemMenu(newItem.element, newItem.isNew);
    }

    void SetItemMenu(PassiveItemData pickingItem, bool isNew)
    {
        _currPickingItem = pickingItem;
        _itemInfoButton.SetValues(null, null, pickingItem);
        _itemImage.sprite = pickingItem.ItemSprite;
        Utility.ScaleImageToFitTarget(_itemImage.rectTransform, pickingItem.ItemSprite, _itemImageTargetSize.sizeDelta);
        if (isNew)
            _currNewIndicator = Instantiate(_newItemIndicator, _itemImageTargetSize);

        _itemName.text = pickingItem.ItemName;
        _itemDescript.text = pickingItem.ItemDescript;
        _rerollsLeftDisplay.text = "" + RerollManager.rm.RerollsLeft;
        _cantPickWeaponButton.SetActive(PlayerControl.pc.PassiveItemManager.GetItemCopies(pickingItem) < pickingItem.ItemMaxCopies);
    }
}
