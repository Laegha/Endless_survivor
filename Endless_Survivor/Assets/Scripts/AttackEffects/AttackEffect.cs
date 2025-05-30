using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackEffect
{
    public static bool isUsable => false;
    [SerializeField] bool _usesSeparateChance = false;
    [SerializeField][Range(0, 100)] float _effectChance = 50;
    public bool UsesSeparateChance { get { return _usesSeparateChance; } }
    public float EffectChance { get { return _effectChance; } }
    public virtual void OnAttack(Weapon attackingWeapon) { }
    public virtual void Update() { }
    public virtual void OnEnemyHit(EnemyControl hitEnemyControl) { }
}
