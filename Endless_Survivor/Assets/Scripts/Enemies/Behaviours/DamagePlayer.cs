using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamagePlayer : EnemyBehaviour
{
    [SerializeField] int _damage;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);

        DamagePlayer originalDamagePlayer = original as DamagePlayer;
        _damage = originalDamagePlayer._damage;

        GameObject damageColliderGO = new GameObject("DamageCollider");
        damageColliderGO.transform.SetParent(enemyControl.transform, false);
        damageColliderGO.transform.localPosition = Vector3.zero;
        CapsuleCollider2D regularCollider = enemyControl.CapsuleCollider;
        CapsuleCollider2D damageCollider = damageColliderGO.AddComponent<CapsuleCollider2D>();

        damageCollider.direction = regularCollider.direction;
        damageCollider.size = regularCollider.size * 0.9f;
        damageCollider.offset = regularCollider.offset;
        damageCollider.isTrigger = true;
        damageColliderGO.layer = LayerMask.NameToLayer("PlayerDetector");
        damageColliderGO.AddComponent<PlayerDamageSource>().Damage = _damage;
        //damageColliderGO.transform.localPosition = regularCollider.transform.localPosition;
    }
}
