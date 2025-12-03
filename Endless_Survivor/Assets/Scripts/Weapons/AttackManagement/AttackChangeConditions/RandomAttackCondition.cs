using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RandomAttackCondition : WeaponAttackChangeCondition
{
    new public static bool isUsable => true;
    [Range(0, 100)][SerializeField] float _changeChance;
    [Tooltip("Use this, you can ignore the array of ids")]
    [SerializeField] List<RouletteElementChance<string>> _idsWeights;
    public override void CopyData(WeaponAttackManager weaponAM, WeaponAttackChangeCondition original)
    {
        base.CopyData(weaponAM, original);
        RandomAttackCondition randomOriginal = original as RandomAttackCondition;
        _changeChance = randomOriginal._changeChance;
        _idsWeights = randomOriginal._idsWeights;
    }
    public override WeaponAttackController GetAttackController()
    {
        var selectedAttackId = Utility.GetRouletteElement<string>(_idsWeights);
        return WeaponAM.AttackControllers.Find(x => x.AttackId == selectedAttackId);
    }
    public override bool IsConditionMet()
    {
        float rand = UnityEngine.Random.Range(0, 100f);
        return (rand <= _changeChance);
    }
}
