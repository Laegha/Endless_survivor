using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStatusEffect
{
    public static bool isUsable => false;
    EnemyControl _affectedEnemyControl;
    [SerializeField] int _effectMaxStacks = 1;
    public EnemyControl AffectedEnemyControl {  get { return _affectedEnemyControl; } }
    public int EffectMaxStacks {  get { return _effectMaxStacks; } }

    public virtual void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        _affectedEnemyControl = affectedEnemyControl;
    }

    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void End() { }
    public virtual void EnemyHit() { }
    public virtual void EnemyKilled() { }
}
