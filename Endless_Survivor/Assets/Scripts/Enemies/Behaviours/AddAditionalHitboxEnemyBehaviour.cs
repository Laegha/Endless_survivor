using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AddAditionalHitboxEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] Vector2 _colliderSize;
    [SerializeField] Vector2 _colliderOffset;
    [SerializeField] CapsuleDirection2D _colliderDirection;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var originalAdditionalHitbox = original as AddAditionalHitboxEnemyBehaviour;
        _colliderSize = originalAdditionalHitbox._colliderSize;
        _colliderOffset = originalAdditionalHitbox._colliderOffset;
        _colliderDirection = originalAdditionalHitbox._colliderDirection;
        GameObject additionalColliderGO = new GameObject("AdditionalCollider");
        additionalColliderGO.transform.SetParent(enemyControl.transform, false);
        additionalColliderGO.transform.localPosition = Vector2.zero;
        CapsuleCollider2D additionalCollider = additionalColliderGO.AddComponent<CapsuleCollider2D>();

        additionalCollider.size = _colliderSize;
        additionalCollider.direction = _colliderDirection;
        additionalCollider.offset = _colliderOffset;
        additionalCollider.isTrigger = true;
        additionalColliderGO.layer = LayerMask.NameToLayer("Enemy");
    }
}
