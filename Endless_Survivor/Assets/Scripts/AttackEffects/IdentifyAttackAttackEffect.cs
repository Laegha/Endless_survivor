using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdentifyAttackAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] string _id;
    public IdentifyAttackAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var identifyOriginal = original as IdentifyAttackAttackEffect;
        _id = identifyOriginal._id;
        ObjectIdentifier.AddIdentifiedObject(_id, affectedAttack.gameObject);
    }
}
