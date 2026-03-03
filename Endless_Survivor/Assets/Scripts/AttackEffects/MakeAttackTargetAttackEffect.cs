using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MakeAttackTargetAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _targetPriority = 1;
    public MakeAttackTargetAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var makeTargetOriginal = original as MakeAttackTargetAttackEffect;
        _targetPriority = makeTargetOriginal._targetPriority;
        AffectedAttack.ParentWeapon.WeaponControl.WeaponAim.ExclusiveAttackTargets.Add(new(affectedAttack.gameObject, _targetPriority));

    }
}
