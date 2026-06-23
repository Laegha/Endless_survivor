using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InformationButton : MonoBehaviour
{
    enum InformationMenuType
    {
        CharacterInfo,
        WeaponInfo,
        ItemInfo
    }

    [SerializeField] InformationMenuType _showingInfoType;
    CharacterData _character;
    WeaponData _weapon;
    PassiveItemData _item;

    public void SetValues(CharacterData character, WeaponData weapon, PassiveItemData item)
    {
        _character = character;
        _weapon = weapon;
        _item = item;
    }

    public void DisplayInfo()
    {
        switch(_showingInfoType)
        {
            case InformationMenuType.CharacterInfo:
                GameUIManager.uiManager.DisplayCharacterInfo(_character);
                break;
            case InformationMenuType.WeaponInfo:
                GameUIManager.uiManager.DisplayWeaponInfo(_weapon);
                break;
            case InformationMenuType.ItemInfo:
                GameUIManager.uiManager.DisplayItemInfo(_item);
                break;
        }
    }
}
