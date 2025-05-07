using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : ScriptableObject
{
    [SerializeField] float _effectChance = 50;
    public virtual void OnAttack(Weapon attackingWeapon) { }
    public virtual void Update() { }
    public virtual void OnEnemyHit(EnemyControl hitEnemyControl) { }
}
