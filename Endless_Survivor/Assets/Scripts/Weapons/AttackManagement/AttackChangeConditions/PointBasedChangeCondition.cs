using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointBasedChangeCondition : WeaponAttackChangeCondition
{
    new public static bool isUsable => true;
    static Dictionary<WeaponAttackManager, int> _weaponsPoints = new Dictionary<WeaponAttackManager, int>();
    static Dictionary<WeaponAttackManager, System.Action> _onPointsReset = new();
    [SerializeField] int _neededPoints;
    [SerializeField] string _triggeredAttackId;
    [SerializeField] bool _usesPointsOnTrigger;

    public static Dictionary<WeaponAttackManager, int> WeaponPoints {  get { return _weaponsPoints; } }
    public static Dictionary<WeaponAttackManager, System.Action> OnPointsReset {  get { return _onPointsReset; } }

    public override void CopyData(WeaponAttackManager weaponAM, WeaponAttackChangeCondition original)
    {
        base.CopyData(weaponAM, original);
        var pointBasedOriginal = original as PointBasedChangeCondition;
        if (!_weaponsPoints.ContainsKey(WeaponAM))
        {
            _weaponsPoints.Add(weaponAM, 0);
            _onPointsReset.Add(weaponAM, null);
        }
        _neededPoints = pointBasedOriginal._neededPoints;
        _triggeredAttackId = pointBasedOriginal._triggeredAttackId;
        _usesPointsOnTrigger = pointBasedOriginal._usesPointsOnTrigger;
    }
    public override bool IsConditionMet()
    {
        bool isMet = _weaponsPoints[WeaponAM] == _neededPoints;
        if (isMet && _usesPointsOnTrigger)
        {
            _onPointsReset[WeaponAM]?.Invoke();
            _weaponsPoints[WeaponAM] = 0;
        }
        return isMet;
    }
    public override WeaponAttackController GetAttackController()
    {
        Debug.Log("Needed points: " + _neededPoints + " triggering attack " +  _triggeredAttackId);
        return WeaponAM.AttackControllers.Find(x => x.AttackId == _triggeredAttackId);
    }
}
