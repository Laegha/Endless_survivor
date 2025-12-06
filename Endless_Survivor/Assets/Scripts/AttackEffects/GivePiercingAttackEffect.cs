using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePiercingAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _piercingGiven;
    public GivePiercingAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }

    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var piercingOriginal = original as GivePiercingAttackEffect;
        _piercingGiven = piercingOriginal._piercingGiven;
        affectedAttack.AttackPiercing += _piercingGiven;
    }
}
