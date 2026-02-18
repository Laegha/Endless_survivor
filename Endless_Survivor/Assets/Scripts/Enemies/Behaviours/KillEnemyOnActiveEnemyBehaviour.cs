using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillEnemyOnActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => 1;
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        int maxHP = EnemyControl.EnemyHP.MaxHP;
        EnemyControl.EnemyHP.TakeDamage(maxHP);
        KillBehaviour();
    }
}
