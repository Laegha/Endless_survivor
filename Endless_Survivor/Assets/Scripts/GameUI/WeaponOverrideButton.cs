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
    [SerializeField] TextMeshProUGUI _trueLevelGfx;
    [SerializeField] TextMeshProUGUI _inducedLevelGfx;
    WeaponAttackManager _assignedWeapon;
    public Action<WeaponAttackManager> _onPressed;

    public void SetData(Sprite weaponGfx, WeaponAttackManager assignedWeapon, Action<WeaponAttackManager> onPressed)
    {
        Utility.ScaleImageToFitTarget(_weaponGfx.rectTransform, weaponGfx, _weaponGfxTargetSize.sizeDelta);
        _weaponGfx.sprite = weaponGfx;
        print("WEAPON " + assignedWeapon + " LEVEL " + assignedWeapon.WeaponStats.TrueLevel);
        _trueLevelGfx.text = "" + assignedWeapon.WeaponStats.TrueLevel;
        if(assignedWeapon.WeaponStats.InducedLevel > 0)
        {
            _inducedLevelGfx.gameObject.SetActive(true);
            _inducedLevelGfx.text = "+" + assignedWeapon.WeaponStats.InducedLevel;
        }
        _assignedWeapon = assignedWeapon;
        _onPressed = onPressed;
    }
    public void SelectWeapon()
    {
        _onPressed.Invoke(_assignedWeapon);
    }
}
