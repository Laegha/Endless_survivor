using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AddPointsAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _pointsAddedOnStart;
    [SerializeField] int _pointsAddedOnEnemyHit;
    [SerializeField] int _pointsAddedOnEnemyKill;

    public AddPointsAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }

    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var addPointsOriginal = original as AddPointsAttackEffect;
        _pointsAddedOnStart = addPointsOriginal._pointsAddedOnStart;
        _pointsAddedOnEnemyHit = addPointsOriginal._pointsAddedOnEnemyHit;
        _pointsAddedOnEnemyKill = addPointsOriginal._pointsAddedOnEnemyKill;

        if (_pointsAddedOnStart != 0)
            OnAttack += AddStartPoints;
        if (_pointsAddedOnEnemyHit != 0)
            OnEnemyHit += AddHitPoints;
        if (_pointsAddedOnEnemyKill != 0)
            OnEnemyHit += AddKillPoints;
    }
    void AddStartPoints() => AddPoints(_pointsAddedOnStart);
    void AddHitPoints(EnemyControl placeholder) => AddPoints(_pointsAddedOnEnemyHit);
    void AddKillPoints(EnemyControl hitEnemy)
    {
        if (hitEnemy.EnemyHP.RemainingHP > AffectedAttack.AttackDamage)
            return;
        AddPoints(_pointsAddedOnStart);
    }
    void AddPoints(int points)
    {
        if (!PointBasedChangeCondition.WeaponPoints.ContainsKey(AffectedAttack.ParentWeapon.WeaponControl.WeaponAttackManager))
            return;
        PointBasedChangeCondition.WeaponPoints[AffectedAttack.ParentWeapon.WeaponControl.WeaponAttackManager] += points;
    }
}
