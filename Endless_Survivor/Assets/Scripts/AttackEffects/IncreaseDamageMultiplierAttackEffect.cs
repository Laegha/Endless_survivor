using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreaseDamageMultiplierAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] float _damageMultiplierIncrease;

    public IncreaseDamageMultiplierAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var increaseDamageMultiplierOriginal = original as IncreaseDamageMultiplierAttackEffect;
        OnAttack += IncreaseDamageMultiplier;
    }

    void IncreaseDamageMultiplier()
    {
        AffectedAttack.AttackDamageMultiplier += _damageMultiplierIncrease;
    }
}
