using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponPickupMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuGfx;
    [SerializeField] Image _weaponImage;
    [SerializeField] TextMeshProUGUI _weaponLevelDisplay;
    WeaponData _currDisplayingWeapon;
    WeaponStats _currWeaponStats;

    public void DisplayWeapon(WeaponData displayingWeapon)
    {
        _currDisplayingWeapon = displayingWeapon;
        _weaponImage.sprite = _currDisplayingWeapon.WeaponDisplaySprite;
        _currWeaponStats = new WeaponStats(_currDisplayingWeapon.WeaponStats);
        _currWeaponStats.ScaleStats(_currDisplayingWeapon.StatsIncreaseScale, WeaponStats.CurrWaveLevel);
        _menuGfx.SetActive(true);
    }
    public void TakeWeapon()
    {
        GameManager.gm.player.GetComponent<PlayerControl>().WeaponManager.PickupGun(_currDisplayingWeapon, _currWeaponStats);
        _menuGfx.SetActive(false);
    }

    public void LeaveWeapon()
    {
        _menuGfx.SetActive(false);

    }
}
