using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AfterCertainAttackCondition : WeaponAttackChangeCondition
{
    new public static bool isUsable => true;
    [SerializeField] string[] _conditionAttacksIds;
    [SerializeField] string _triggeredAttackId;
    public override void CopyData(WeaponAttackManager weaponAM, WeaponAttackChangeCondition original)
    {
        base.CopyData(weaponAM, original);
        var afterCertainOriginal = original as AfterCertainAttackCondition;
        _conditionAttacksIds = afterCertainOriginal._conditionAttacksIds;
        _triggeredAttackId = afterCertainOriginal._triggeredAttackId;
    }
    public override WeaponAttackController GetAttackController()
    {
        return WeaponAM.AttackControllers.Find(x => x.AttackId == _triggeredAttackId);
    }
    public override bool IsConditionMet()
    {
        return _conditionAttacksIds.Contains(WeaponAM.CurrAttackController?.AttackId);
    }
}
