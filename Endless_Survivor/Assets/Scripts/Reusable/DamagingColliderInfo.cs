using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DamagingColliderInfo
{
    [SerializeField] ColliderInfo _colliderInfo;
    [SerializeField] bool _damagePlayer;
    [SerializeField] bool _damageEnemy;
    [SerializeField] int _damageAmmount;

    DamagingColliderInfo(ColliderInfo colliderInfo, bool damagePlayer, bool damageEnemy, int damageAmmount)
    {
        _colliderInfo = new(colliderInfo);
        _damagePlayer = damagePlayer;
        _damageEnemy = damageEnemy;
        _damageAmmount = damageAmmount;
    }
    DamagingColliderInfo(DamagingColliderInfo original)
    {
        _colliderInfo = new(original._colliderInfo);
        _damagePlayer = original._damagePlayer;
        _damageEnemy = original._damageEnemy;
        _damageAmmount = original._damageAmmount;
    }

    public GameObject GenerateDamagingColliderObj(Transform colliderParent)
    {
        GameObject collider = _colliderInfo.GenerateColliderObj(colliderParent);
        if (_damagePlayer)
        {
            var damageSource = collider.AddComponent<PlayerDamageSource>();
            damageSource.Damage = _damageAmmount;

        }
        if (_damageEnemy)
        {
            var damageSource = collider.AddComponent<EnemyDamageSource>();
            damageSource.Damage = _damageAmmount;

        }
        return collider;
    }
}
