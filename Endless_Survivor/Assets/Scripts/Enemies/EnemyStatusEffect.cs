using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStatusEffect
{
    public static bool isUsable => false;
    EnemyStatusEffectGroup _thisGroup;
    EnemyControl _affectedEnemyControl;
    Func<bool> _endCondition;
    bool _ended = false;
    public EnemyStatusEffectGroup ThisGroup { get { return _thisGroup; }  set { _thisGroup = value; } }
    public EnemyControl AffectedEnemyControl {  get { return _affectedEnemyControl; } }
    public Func<bool> EndCondition { get { return _endCondition; } set { _endCondition = value; } }

    public virtual void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        _affectedEnemyControl = affectedEnemyControl;
    }

    public virtual void Start() { }
    public virtual void Update() 
    {
        if(_endCondition != null && _endCondition() && !_ended)
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
