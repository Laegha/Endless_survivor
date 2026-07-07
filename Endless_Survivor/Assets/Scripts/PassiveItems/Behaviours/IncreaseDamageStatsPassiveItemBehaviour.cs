using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDamageStatsPassiveItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] float _normalMultiplierIncrease;
    [SerializeField] SerializedDictionary<DamageInfo.DamageType, float> _typesMultipliersIncrease;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var increaseDamageOriginal = original as IncreaseDamageStatsPassiveItemBehaviour;
        _normalMultiplierIncrease = increaseDamageOriginal._normalMultiplierIncrease;
        _typesMultipliersIncrease = increaseDamageOriginal._typesMultipliersIncrease;
    }
    void IncreaseStats()
    {
        DamageTypesMultipliersManager.DamageTypesManager.IncreaseNormalMultiplier(_normalMultiplierIncrease);
        foreach (var damageTypeIncrease in _typesMultipliersIncrease)
        {
            DamageTypesMultipliersManager.DamageTypesManager.IncreaseTypeMultipliers(damageTypeIncrease.Key, damageTypeIncrease.Value);

        }

    }

    public override void RemoveBehaviour()
    {
        DamageTypesMultipliersManager.DamageTypesManager.IncreaseNormalMultiplier(-_normalMultiplierIncrease);
        foreach (var damageTypeIncrease in _typesMultipliersIncrease)
        {
            DamageTypesMultipliersManager.DamageTypesManager.IncreaseTypeMultipliers(damageTypeIncrease.Key, -damageTypeIncrease.Value);

        }
    }
}
