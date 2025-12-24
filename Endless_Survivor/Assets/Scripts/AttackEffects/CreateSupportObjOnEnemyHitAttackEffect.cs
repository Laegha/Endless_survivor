using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSupportObjOnEnemyHitAttackEffect : AttackEffect
{
    [SerializeField] SupportObjectData _createdSupportObj;
    new public static bool isUsable => true;
    public CreateSupportObjOnEnemyHitAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);

        var createSupportObjOriginal = original as CreateSupportObjOnEnemyHitAttackEffect;
        _createdSupportObj = createSupportObjOriginal._createdSupportObj;
        OnEnemyHit += CreateSupportObjOnEnemyPos;
    }
    void CreateSupportObjOnEnemyPos(EnemyControl hitEnemy)
    {
        Utility.GenerateSupportObj(_createdSupportObj, AffectedAttack.transform.position, Quaternion.identity);
    }
}
