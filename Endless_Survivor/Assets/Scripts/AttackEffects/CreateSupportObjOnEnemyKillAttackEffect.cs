using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateSupportObjOnEnemyKillAttackEffect : AttackEffect
{
    [SerializeField] SupportObjectData _createdSupportObj;
    static List<EnemyControl> _addedEnemies = new List<EnemyControl>();
    new public static bool isUsable => true;
    public CreateSupportObjOnEnemyKillAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);

        var createSupportObjOriginal = original as CreateSupportObjOnEnemyKillAttackEffect;
        _createdSupportObj = createSupportObjOriginal._createdSupportObj;
        OnEnemyHit += AddActionToEnemyDeath;
    }
    void AddActionToEnemyDeath(EnemyControl hitEnemy)
    {
        if (_addedEnemies.Contains(hitEnemy)) return;
        hitEnemy.EnemyHP.OnDeath += CreateSupportObjOnEnemyPos;
        _addedEnemies.Add(hitEnemy);
    }

    void CreateSupportObjOnEnemyPos(EnemyControl hitEnemy)
    {
        Utility.GenerateSupportObj(_createdSupportObj, hitEnemy.transform.position, Quaternion.identity);
        _addedEnemies.Remove(hitEnemy);
    }
}
