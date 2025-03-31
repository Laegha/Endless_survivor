using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponPickupMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuGfx;
    [SerializeField] Image _weaponImage;
    [SerializeField] RectTransform _weaponImageTargetSize;
    [SerializeField] TextMeshProUGUI _weaponLevelDisplay;
    WeaponData _currDisplayingWeapon;
    WeaponStats _currWeaponStats;

    public WeaponData CurrDisplayingWeapon {  get { return _currDisplayingWeapon; } }
    public WeaponStats CurrWeaponStats {  get { return _currWeaponStats; } }

    public void DisplayMenu(WeaponData displayingWeapon)
    {
        //stop game
        _menuGfx.SetActive(true);
        _currDisplayingWeapon = displayingWeapon;
        Utility.ScaleImageToFitTarget(_weaponImage.rectTransform, displayingWeapon.WeaponDisplaySprite, _weaponImageTargetSize.sizeDelta);
        _weaponImage.sprite = _currDisplayingWeapon.WeaponDisplaySprite;
        _currWeaponStats = new WeaponStats(_currDisplayingWeapon.WeaponStats);
        _currWeaponStats.ScaleStats(_currDisplayingWeapon.StatsIncreaseScale, WeaponStats.CurrWaveLevel);
    }
    public void TakeWeapon()
    {
        GameManager.gm.player.GetComponent<PlayerControl>().WeaponManager.PickupWeapon(_currDisplayingWeapon, _currWeaponStats);
        _menuGfx.SetActive(false);
    }

    public void DiscardWeapon()
    {
        _menuGfx.SetActive(false);

    }
}
