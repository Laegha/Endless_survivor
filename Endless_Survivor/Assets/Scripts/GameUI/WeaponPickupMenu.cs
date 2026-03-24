using System;
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
    [SerializeField] TextMeshProUGUI _weaponName;
    [SerializeField] TextMeshProUGUI _weaponLevelDisplay;
    [SerializeField] TextMeshProUGUI _weaponDmgDisplay;
    [SerializeField] TextMeshProUGUI _weaponAtkSpdDisplay;
    [SerializeField] TextMeshProUGUI _weaponRangeDisplay;
    [SerializeField] TextMeshProUGUI _rerollsLeftDisplay;
    WeaponData _currDisplayingWeapon;
    WeaponStats _currWeaponStats;
    GameObject _currNewIndicator;
    Action _onMenuOpen;

    public WeaponData CurrDisplayingWeapon {  get { return _currDisplayingWeapon; } }
    public WeaponStats CurrWeaponStats {  get { return _currWeaponStats; } }
    public Action OnMenuOpen { get { return _onMenuOpen; } set { _onMenuOpen = value; } }

    public void DisplayMenu(WeaponData displayingWeapon, bool isNew)
    {
        //stop game
        _menuGfx.SetActive(true);
        GameUIManager.uiManager.MenuDisplayed();

        _onMenuOpen?.Invoke();
        SetMenuWeapon(displayingWeapon, isNew);

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

    public void RerollWeapon()
    {
        if (RerollManager.rm.RerollsLeft <= 0)
            return;
        RerollManager.rm.UseReroll();
        var newWeapon = RandomWeaponGetter.GetWeapon(true, _currDisplayingWeapon.WeaponPools);
        while(newWeapon.element == _currDisplayingWeapon)
            newWeapon = RandomWeaponGetter.GetWeapon(true, _currDisplayingWeapon.WeaponPools);

        SetMenuWeapon(newWeapon.element, newWeapon.isNew);
    }
    void SetMenuWeapon(WeaponData displayingWeapon, bool isNew)
    {
        _currDisplayingWeapon = displayingWeapon;

        Utility.ScaleImageToFitTarget(_weaponImage.rectTransform, displayingWeapon.WeaponDisplaySprite, _weaponImageTargetSize.sizeDelta);
        if (isNew)
            _currNewIndicator = Instantiate(_newWeaponIndicator, _weaponImageTargetSize);

        _weaponImage.sprite = displayingWeapon.WeaponDisplaySprite;

        _currWeaponStats = new WeaponStats(_currDisplayingWeapon.WeaponStats);
        _currWeaponStats.SetTrueLevelStats(_currDisplayingWeapon.StatsIncreaseScale, ScalingFunctions.CurrScalingLevel);

        _weaponName.text = _currDisplayingWeapon.WeaponName;
        _weaponLevelDisplay.text = _currWeaponStats.TrueLevel + "";
        _weaponDmgDisplay.text = _currWeaponStats.Damage + "";
        _weaponAtkSpdDisplay.text = Utility.ChangeFloatDecimals(_currWeaponStats.AttackSpeed, 2) + "";
        _weaponRangeDisplay.text = _currWeaponStats.Range + "";
        _rerollsLeftDisplay.text = ""+RerollManager.rm.RerollsLeft;
    }
}
