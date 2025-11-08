using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStatusEffect
{
    public static bool isUsable => false;
    EnemyControl _affectedEnemyControl;
    [SerializeField] int _effectMaxStacks = 1; //i'm pretty sure this should be in the scriptable object and MAYBE allow for a specific effect to stack a different ammount
    Func<bool> _endCondition;
    bool _ended = false;
    public EnemyControl AffectedEnemyControl {  get { return _affectedEnemyControl; } }
    public int EffectMaxStacks {  get { return _effectMaxStacks; } }
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
