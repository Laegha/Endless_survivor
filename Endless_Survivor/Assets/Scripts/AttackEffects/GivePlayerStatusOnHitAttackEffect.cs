using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GivePlayerStatusOnHitAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] PlayerStatusEffectData _givenEffectData;
    public GivePlayerStatusOnHitAttackEffect(AttackEffect original, Attack affectedAttack) :base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var givePlayerStatusOriginal = original as GivePlayerStatusOnHitAttackEffect;
        _givenEffectData = givePlayerStatusOriginal._givenEffectData;
        OnEnemyHit += GiveStatus;
    }
    void GiveStatus(EnemyControl placeholder)
    {
        PlayerControl.pc.StatusEffectManager.AddEffects(_givenEffectData.StatusEffects, _givenEffectData);
    }
}
