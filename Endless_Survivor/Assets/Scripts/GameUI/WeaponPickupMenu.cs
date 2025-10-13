using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickupMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuGfx;
    [SerializeField] GameObject _newWeaponIndicator;
    [SerializeField] Image _weaponImage;
    [SerializeField] RectTransform _weaponImageTargetSize;
    [SerializeField] TextMeshProUGUI _weaponLevelDisplay;
    [SerializeField] TextMeshProUGUI _weaponDmgDisplay;
    [SerializeField] TextMeshProUGUI _weaponAtkSpdDisplay;
    [SerializeField] TextMeshProUGUI _weaponRangeDisplay;
    WeaponData _currDisplayingWeapon;
    WeaponStats _currWeaponStats;
    GameObject _currNewIndicator;

    public WeaponData CurrDisplayingWeapon {  get { return _currDisplayingWeapon; } }
    public WeaponStats CurrWeaponStats {  get { return _currWeaponStats; } }

    public void DisplayMenu(WeaponData displayingWeapon, bool isNew)
    {
        //stop game
        _menuGfx.SetActive(true);
        GameUIManager.uiManager.MenuDisplayed();
        _currDisplayingWeapon = displayingWeapon;
        Utility.ScaleImageToFitTarget(_weaponImage.rectTransform, displayingWeapon.WeaponDisplaySprite, _weaponImageTargetSize.sizeDelta);
        _weaponImage.sprite = displayingWeapon.WeaponDisplaySprite;
        if (isNew)
            _currNewIndicator = Instantiate(_newWeaponIndicator, _weaponImageTargetSize);

        _currWeaponStats = new WeaponStats(_currDisplayingWeapon.WeaponStats);
        _currWeaponStats.SetTrueLevelStats(_currDisplayingWeapon.StatsIncreaseScale, ScalingFunctions.CurrWaveLevel);
        _weaponLevelDisplay.text = _currWeaponStats.TrueLevel + "";
        _weaponDmgDisplay.text = _currWeaponStats.Damage + "";
        _weaponAtkSpdDisplay.text = Utility.ChangeFloatDecimals(_currWeaponStats.AttackSpeed, 2) + "";
        _weaponRangeDisplay.text = _currWeaponStats.Range + "";

    }
    public void TakeWeapon()
    {
        GameUIManager.uiManager.MenuHid();
        PlayerControl.pc.WeaponManager.PickupWeapon(_currDisplayingWeapon, _currWeaponStats);
        if(_currNewIndicator != null)
        {
            UnlockmentsManager.SetNotNewWeapon(_currDisplayingWeapon);
            Destroy(_currNewIndicator);
        }
        _menuGfx.SetActive(false);
    }

    public void DiscardWeapon()
    {
        GameUIManager.uiManager.MenuHid();
        _menuGfx.SetActive(false);
    }
}
