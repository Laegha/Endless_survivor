using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class WeaponOverrideButton : MonoBehaviour
{
    [SerializeField] Image _weaponGfx;
    [SerializeField] TextMeshProUGUI _levelGfx;
    Weapon _assignedWeapon;
    public Action<Weapon> _onPressed;

    public void SetData(Sprite weaponGfx, Weapon assignedWeapon, Action<Weapon> onPressed)
    {
        _weaponGfx.sprite = weaponGfx;
        _levelGfx.text = "" + assignedWeapon.WeaponStats.Level;
        _assignedWeapon = assignedWeapon;
        _onPressed = onPressed;
    }
    public void SelectWeapon()
    {
        _onPressed.Invoke(_assignedWeapon);
    }
}
