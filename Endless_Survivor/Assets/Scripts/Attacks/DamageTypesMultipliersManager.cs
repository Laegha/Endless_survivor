using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DamageTypesMultipliersManager : MonoBehaviour
{
    static DamageTypesMultipliersManager instance;
    public static DamageTypesMultipliersManager DamageTypesManager
    {
        get
        {
            return instance;
        }
    }
    float _normalDamageMultiplier = 1;
    Dictionary<DamageInfo.DamageType, float> _typesMultipliers = new();

    private void Awake()
    {
        instance = this;
        foreach (var item in Enum.GetValues(typeof(DamageInfo.DamageType)))
        {
            var damageType = item.ConvertTo<DamageInfo.DamageType>();
            _typesMultipliers.Add(damageType, 1);
        }


    }

    public float GetMultipliers(DamageInfo.DamageType type)
    {
        float resultMultiplier = _normalDamageMultiplier;
        foreach(var typeMultiplier in _typesMultipliers)
        {
            if (!type.HasFlag(typeMultiplier.Key))
                continue;
            resultMultiplier *= typeMultiplier.Value;

        }
        return resultMultiplier;
    }

    public void IncreaseNormalMultiplier(float multiplierIncrease)
    {
        _normalDamageMultiplier += multiplierIncrease;
    }
    public void IncreaseTypeMultipliers(DamageInfo.DamageType type, float increase)
    {
        List<DamageInfo.DamageType> keys = new(_typesMultipliers.Keys);
        foreach (var typeKey in keys)
        {
            if (!type.HasFlag(typeKey))
                continue;
            _typesMultipliers[typeKey] += increase;

        }
    }
}
