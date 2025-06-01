using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ApplyEnemyStatusOnHitAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeReference] EnemyStatusEffect _appliedStatusEffect;
    public EnemyStatusEffect AppliedStatusEffect {get { return _appliedStatusEffect; } set {  _appliedStatusEffect = value; } }
    
    public ApplyEnemyStatusOnHitAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack)
    {
        base.OnEnemyHit += ApplyStatus;

    }
    
    public void ApplyStatus(EnemyControl hitEnemyControl)
    {
        hitEnemyControl.StatusEffectManager.AddEffect(_appliedStatusEffect);
    }
}
