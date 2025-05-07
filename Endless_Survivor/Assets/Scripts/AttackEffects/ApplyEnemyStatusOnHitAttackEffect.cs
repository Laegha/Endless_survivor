using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyStatusAtkEffect", menuName = "ScriptableObjects/AttackEffects/EnemyStatusAtkEffect", order = 1)]
public class ApplyEnemyStatusOnHitAttackEffect : AttackEffect
{
    [SerializeReference] EnemyStatusEffect _appliedStatusEffect;
    public EnemyStatusEffect AppliedStatusEffect {get { return _appliedStatusEffect; } set {  _appliedStatusEffect = value; } }
    public override void OnEnemyHit(EnemyControl hitEnemyControl)
    {
        base.OnEnemyHit(hitEnemyControl);
        hitEnemyControl.StatusEffectManager.AddEffect(_appliedStatusEffect);
    }
}
