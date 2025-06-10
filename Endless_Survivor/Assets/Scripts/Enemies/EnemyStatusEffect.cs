using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemyStatusEffect
{
    //can be duplicated bool (should be virtual or smth)
    EnemyControl _affectedEnemyControl;
    [SerializeField] Sprite _statusEffectIndicator;
    [SerializeField] Material _statusEffectMaterial;
    public EnemyControl AffectedEnemyControl {  get { return _affectedEnemyControl; } }

    public virtual void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        _affectedEnemyControl = affectedEnemyControl;
        _statusEffectIndicator = original._statusEffectIndicator;
        _statusEffectMaterial = original._statusEffectMaterial;
    }

    public virtual void Start() 
    {
        //show indicator and change material
        _affectedEnemyControl.StatusEffectManager.AddStatusGraphics(_statusEffectIndicator, _statusEffectMaterial, this);
    }
    public virtual void Update() { }
    public virtual void End() { }
    public virtual void EnemyHit() { }
    public virtual void EnemyKilled() { }
}
