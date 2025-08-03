using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemPickupMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuGfx;
    PassiveItemData _currPickingItem;

    public void DisplayMenu(PassiveItemData pickingItem)
    {
        _menuGfx.SetActive(true);
        GameUIManager.uiManager.MenuDisplayed();
        _currPickingItem = pickingItem;
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
    }
}
