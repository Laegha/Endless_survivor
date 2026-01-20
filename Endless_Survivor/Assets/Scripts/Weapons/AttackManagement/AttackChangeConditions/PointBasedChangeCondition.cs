using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBasedChangeCondition : WeaponAttackChangeCondition
{
    static Dictionary<WeaponAttackManager, int> _weaponsPoints = new Dictionary<WeaponAttackManager, int>();
    [SerializeField] int _neededPoints;
    [SerializeField] string _triggeredAttackId;

    public static Dictionary<WeaponAttackManager, int> WeaponPoints {  get { return _weaponsPoints; } }

    public override void CopyData(WeaponAttackManager weaponAM, WeaponAttackChangeCondition original)
    {
        base.CopyData(weaponAM, original);
        var pointBasedOriginal = original as PointBasedChangeCondition;
        if (!_weaponsPoints.ContainsKey(WeaponAM))
            _weaponsPoints.Add(weaponAM, 0);
        _neededPoints = pointBasedOriginal._neededPoints;
        _triggeredAttackId = pointBasedOriginal._triggeredAttackId;
    }
    public override bool IsConditionMet()
    {
        return _weaponsPoints[WeaponAM] == _neededPoints;
    }
    public override WeaponAttackController GetAttackController()
    {
        return WeaponAM.AttackControllers.Find(x => x.AttackId == _triggeredAttackId);
    }
}
