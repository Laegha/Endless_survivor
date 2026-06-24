using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponInfoMenu : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _weaponNameDisplay;
    [SerializeField] ScalableToTargetImage _weaponImage;

    [InspectorLabel("WeaponStats")]
    [SerializeField] TextMeshProUGUI _rangeDisplay;
    [SerializeField] TextMeshProUGUI _damageDisplay;
    [SerializeField] TextMeshProUGUI _attackSpeedDisplay;
    [SerializeField] TextMeshProUGUI _knockbackDisplay;

    [InspectorLabel("Damage types")]
    [SerializeField] RectTransform _damageTypesRow;
    [SerializeField] ScalableToTargetImage _damageTypeImagePrefab;

    List<GameObject> _displayingDamageTypes = new();

    public void UpdateInfo(WeaponData newShowingWeapon)
    {
        _weaponNameDisplay.text = newShowingWeapon.WeaponName;
        _weaponImage.ChangeImageSprite(newShowingWeapon.WeaponDisplaySprite);

        _rangeDisplay.text = newShowingWeapon.WeaponStats.Range + "";
        _damageDisplay.text = newShowingWeapon.WeaponStats.Damage + "";
        _attackSpeedDisplay.text = newShowingWeapon.WeaponStats.AttackSpeed + "";
        _knockbackDisplay.text = newShowingWeapon.WeaponStats.Knockback + "";

        var displayingDamageTypesCopy = new List<GameObject>(_displayingDamageTypes);
        foreach(var existingDamageType in displayingDamageTypesCopy)
        {
            _displayingDamageTypes.Remove(existingDamageType);
            Destroy(existingDamageType);
        }

        DamageInfo.DamageType[] damageTypes = (DamageInfo.DamageType[])Enum.GetValues(typeof(DamageInfo.DamageType));
        foreach (var damageType in damageTypes)
        {
            if (!newShowingWeapon.DefaultAttack.DamageType.HasFlag(damageType) || !GameManager.gm.WeaponStatsSprites.DamageTypesSprites.ContainsKey(damageType))
                continue;
            var damageTypeDisplay = Instantiate(_damageTypeImagePrefab);
            Sprite damageTypeSprite = GameManager.gm.WeaponStatsSprites.DamageTypesSprites[damageType];
            damageTypeDisplay.ChangeImageSprite(damageTypeSprite);
            damageTypeDisplay.GetComponent<RectTransform>().SetParent(_damageTypesRow);
            damageTypeDisplay.GetComponent<RectTransform>().localScale = Vector3.one;
            _displayingDamageTypes.Add(damageTypeDisplay.gameObject);
        }
}
}
