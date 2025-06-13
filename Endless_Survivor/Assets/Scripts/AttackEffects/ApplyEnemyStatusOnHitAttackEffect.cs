using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ApplyEnemyStatusOnHitAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] EnemyStatusEffectData _statusEffectData;
    
    public ApplyEnemyStatusOnHitAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }

    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var statusEffectOriginal = (ApplyEnemyStatusOnHitAttackEffect)original;
        _statusEffectData = statusEffectOriginal._statusEffectData;
        OnEnemyHit += ApplyStatus;

    }
    
    public void ApplyStatus(EnemyControl hitEnemyControl)
    {
        _statusEffectData.ApplyEffects(hitEnemyControl.StatusEffectManager);
    }
}
