using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddKnockbackMultiplierToAttackAttackEffect : AttackEffect
{
    new public bool isUsable => true;
    [SerializeField] float _addedKnockbackMultiplier;
    public AddKnockbackMultiplierToAttackAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var addKnockbackOriginal = original as AddKnockbackMultiplierToAttackAttackEffect;
        _addedKnockbackMultiplier = addKnockbackOriginal._addedKnockbackMultiplier;
        OnAttack += AddKnockback;
    }
    void AddKnockback()
    {

        AffectedAttack.AttackKnockbackMultiplier = Mathf.Clamp(AffectedAttack.AttackKnockbackMultiplier + _addedKnockbackMultiplier, 0, Mathf.Infinity);
    }
}
