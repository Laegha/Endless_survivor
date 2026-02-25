using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AddPointsWithTimeAttackChangeCondition : WeaponAttackChangeCondition
{
    new public static bool isUsable => true;
    [SerializeField] float _timeToIncreasePoints;
    [SerializeField] int _pointsAdded;
    [SerializeField] int _pointLimit;

    float _timer = 0;
    public override void CopyData(WeaponAttackManager weaponAM, WeaponAttackChangeCondition original)
    {
        base.CopyData(weaponAM, original);
        var addPointsOriginal = original as AddPointsWithTimeAttackChangeCondition;
        _timeToIncreasePoints = addPointsOriginal._timeToIncreasePoints;
        _pointsAdded = addPointsOriginal._pointsAdded;
        _pointLimit = addPointsOriginal._pointLimit;
        
        GameManager.gm.RoutineRunner(Update());
    }
    public override bool IsConditionMet()
    {
        return false;
    }
    public override WeaponAttackController GetAttackController()
    {
        return null;
    }

    IEnumerator Update()
    {
        yield return new WaitForEndOfFrame();
        if (!PointBasedChangeCondition.WeaponPoints.ContainsKey(WeaponAM))//this should never happen, but just in case
            yield break;
        PointBasedChangeCondition.OnPointsReset[WeaponAM] += () => _timer = 0;
        while(WeaponAM != null)
        {
            Debug.Log("POINTS " + PointBasedChangeCondition.WeaponPoints[WeaponAM]);
            yield return null;

            if (PointBasedChangeCondition.WeaponPoints[WeaponAM] >= _pointLimit)
            {
                _timer = 0;
                continue;
            }


            _timer += Time.deltaTime;
            if (_timer < _timeToIncreasePoints)
                continue;
            _timer = 0;
            PointBasedChangeCondition.WeaponPoints[WeaponAM] += _pointsAdded;
        }
    }

}
