using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitchMenu : MonoBehaviour
{
    [SerializeField] GameObject _weaponButtonPrefab;
    [SerializeField] GameObject _menuGfx;
    [SerializeField] Image _pickingWeaponImage;
    [SerializeField] RectTransform _pickingWeaponImageTargetSize;
    [SerializeField] TextMeshProUGUI _pickingWeaponLevelDisplay;

    [SerializeField] Transform _weaponDisplayCircleCenter;
    [SerializeField] float _weaponDisplayCircleRadius;
    List<GameObject> _generatedButtons = new List<GameObject>();
    Action<WeaponAttackManager> _onWeaponSelected;
    public void DisplayMenu(List<WeaponAttackManager> holdingWeapons, WeaponData pickingWeapon, WeaponStats pickingWeaponStats, Action<WeaponAttackManager> onWeaponSelected)
    {
        _menuGfx.SetActive(true);
        GameUIManager.uiManager.MenuDisplayed();
        _onWeaponSelected = onWeaponSelected;

        Utility.ScaleImageToFitTarget(_pickingWeaponImage.rectTransform, pickingWeapon.WeaponDisplaySprite, _pickingWeaponImageTargetSize.sizeDelta);
        _pickingWeaponImage.sprite = pickingWeapon.WeaponDisplaySprite;
        _pickingWeaponLevelDisplay.text = pickingWeaponStats.TrueLevel + "";

        float angleStep = 360 / holdingWeapons.Count;
        float currAngle = 0;
        foreach (var weapon in holdingWeapons)
        {
            RectTransform button = Instantiate(_weaponButtonPrefab, _weaponDisplayCircleCenter).GetComponent<RectTransform>();
            button.GetComponent<WeaponOverrideButton>().SetData(weapon.WeaponData.WeaponDisplaySprite, weapon, WeaponSelected);
            button.localPosition = Utility.GetPointInCircle(_weaponDisplayCircleRadius, currAngle);
            currAngle += angleStep;
            _generatedButtons.Add(button.gameObject);
        }

    }

    void WeaponSelected(WeaponAttackManager selectedWeapon)
    {
        CloseMenu();
        _onWeaponSelected?.Invoke(selectedWeapon);
    }

    public void DiscardWeapon()
    { 
        CloseMenu();
    }

    void CloseMenu()
    {
        //re-start game
        _menuGfx.SetActive(false);
        GameUIManager.uiManager.MenuHid();
        foreach (var button in _generatedButtons)
        {
            Destroy(button);
        }
        _generatedButtons.Clear();
    }

}
