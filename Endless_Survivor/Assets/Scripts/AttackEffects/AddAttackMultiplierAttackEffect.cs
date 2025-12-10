using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddAttackMultiplierAttackEffect : AttackEffect
{   
    new public static bool isUsable => true;
    [SerializeField] float _addedMultiplier;
    public AddAttackMultiplierAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var originalAddMultiplier = original as AddAttackMultiplierAttackEffect;
        _addedMultiplier = originalAddMultiplier._addedMultiplier;
        OnAttack += AddMultiplier;
    }

    void AddMultiplier()
    {
        AffectedAttack.AttackDamageMultiplier += _addedMultiplier;
    }
}
