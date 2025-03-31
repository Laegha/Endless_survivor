using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSwitchMenu : MonoBehaviour
{
    [SerializeField] GameObject _weaponButtonPrefab;
    [SerializeField] GameObject _menuGfx;
    [SerializeField] Transform _weaponDisplayCircleCenter;
    [SerializeField] float _weaponDisplayCircleRadius;
    List<GameObject> _generatedButtons = new List<GameObject>();
    Action<Weapon> _onWeaponSelected;

    public void DisplayMenu(List<Weapon> holdingWeapons, Action<Weapon> onWeaponSelected)
    {
        _menuGfx.SetActive(true);
        _onWeaponSelected = onWeaponSelected;
        float angleStep = 360 / holdingWeapons.Count;
        float currAngle = 0;

        foreach (var weapon in holdingWeapons)
        {
            Transform button = Instantiate(_weaponButtonPrefab).transform;
            button.SetParent(_menuGfx.transform);
            button.GetComponent<WeaponOverrideButton>().SetData(weapon.WeaponData.WeaponDisplaySprite, weapon, WeaponSelected);
            button.position = (Vector2)_weaponDisplayCircleCenter.position + Utility.GetPointInCircle(_weaponDisplayCircleRadius, currAngle);
            currAngle += angleStep;
            _generatedButtons.Add(button.gameObject);
        }
    }

    void WeaponSelected(Weapon selectedWeapon)
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
        foreach (var button in _generatedButtons)
        {
            Destroy(button);
        }
        _generatedButtons.Clear();
    }

}
