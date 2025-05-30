using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ApplyEnemyStatusOnHitAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeReference] EnemyStatusEffect _appliedStatusEffect;
    public EnemyStatusEffect AppliedStatusEffect {get { return _appliedStatusEffect; } set {  _appliedStatusEffect = value; } }
    public override void OnEnemyHit(EnemyControl hitEnemyControl)
    {
        base.OnEnemyHit(hitEnemyControl);
        hitEnemyControl.StatusEffectManager.AddEffect(_appliedStatusEffect);
    }
}
