using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LifeStealAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [Range(0, 100)][SerializeField] float _dmgPercentConvertedToHp;

    public LifeStealAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { } 
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var lifeStealOriginal = original as LifeStealAttackEffect;
        _dmgPercentConvertedToHp = lifeStealOriginal._dmgPercentConvertedToHp;
        OnEnemyHit += ConvertDmgToHp;
    }
    void ConvertDmgToHp(EnemyControl _)
    {
        int healedHp = (int)Mathf.Ceil(AffectedAttack.AttackDamage * AffectedAttack.AttackDamageMultiplier * 100 / _dmgPercentConvertedToHp);
        PlayerControl.pc.PlayerHPManager.Heal(healedHp);
    }
}
