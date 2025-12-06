using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerOnEnemyHitAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _healAmmount;
    public HealPlayerOnEnemyHitAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }

    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var healPlayerOriginal = original as HealPlayerOnEnemyHitAttackEffect;
        _healAmmount = healPlayerOriginal._healAmmount;
        OnEnemyHit += HealPlayer;
    }
    void HealPlayer(EnemyControl hitEnemy)
    {
        PlayerControl.pc.PlayerHPManager.Heal(_healAmmount);
    }
}
