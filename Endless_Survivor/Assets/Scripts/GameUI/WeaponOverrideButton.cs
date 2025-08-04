using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponOverrideButton : MonoBehaviour
{
    [SerializeField] Image _weaponGfx;
    [SerializeField] RectTransform _weaponGfxTargetSize;
    [SerializeField] TextMeshProUGUI _levelGfx;
    Weapon _assignedWeapon;
    public Action<Weapon> _onPressed;

    public void SetData(Sprite weaponGfx, Weapon assignedWeapon, Action<Weapon> onPressed)
    {
        Utility.ScaleImageToFitTarget(_weaponGfx.rectTransform, weaponGfx, _weaponGfxTargetSize.sizeDelta);
        _weaponGfx.sprite = weaponGfx;
        _levelGfx.text = "" + assignedWeapon.WeaponStats.TrueLevel;
        _assignedWeapon = assignedWeapon;
        _onPressed = onPressed;
    }
    public void SelectWeapon()
    {
        print("Presseed button");
        _onPressed.Invoke(_assignedWeapon);
    }
}
