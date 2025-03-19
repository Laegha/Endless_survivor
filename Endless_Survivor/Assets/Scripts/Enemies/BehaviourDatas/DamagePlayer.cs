using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamagePlayer : EnemyBehaviour
{
    [SerializeReference] int _damage;

    public DamagePlayer(DamagePlayer original) : base(original)
    {
        _damage = original._damage;
    }

    public override void Start(EnemyControl enemyControl)
    {
        base.Start(enemyControl);

        GameObject damageColliderGO = new GameObject("DamageCollider");
        damageColliderGO.transform.SetParent(enemyControl.transform, false);
        CapsuleCollider2D regularCollider = enemyControl.CapsuleCollider;
        CapsuleCollider2D damageCollider = damageColliderGO.AddComponent<CapsuleCollider2D>();

        damageCollider.direction = regularCollider.direction;
        damageCollider.size = regularCollider.size * 0.9f;
        damageCollider.offset= regularCollider.offset;
        damageCollider.isTrigger = true;
        damageColliderGO.AddComponent<PlayerDamageSource>().Damage = _damage;
        damageColliderGO.transform.localPosition = regularCollider.transform.localPosition;
    }
}
