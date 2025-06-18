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
    ConditionHolder _endCondition;
    bool _ended = false;
    public EnemyControl AffectedEnemyControl {  get { return _affectedEnemyControl; } }
    public int EffectMaxStacks {  get { return _effectMaxStacks; } }
    public ConditionHolder EndCondition { get { return _endCondition; } set { _endCondition = value; } }

    public virtual void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original, ConditionHolder endCondition)
    {
        _affectedEnemyControl = affectedEnemyControl;
        _endCondition = endCondition;
    }

    public virtual void Start() { }
    public virtual void Update() 
    {
        if(_endCondition.value && !_ended)
        {
            _ended = true;
            _affectedEnemyControl.StatusEffectManager.RemoveEffect(this);
            return;
        }
    }
    public virtual void End() { }
    public virtual void EnemyHit() { }
    public virtual void EnemyKilled() { }
}
