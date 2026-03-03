using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseHPSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 0;
    GiveHPSupportObjBehaviour _hpBehaviour;
    Action _onDamage;
    Action _onHeal;
    Action _onDeath;

    public GiveHPSupportObjBehaviour HpBehaviour { get { return _hpBehaviour; } }
    public Action OnDamage {  get { return _onDamage; } set { _onDamage = value; } }
    public Action OnHeal { get { return _onHeal; } set { _onHeal = value; } }
    public Action OnDeath { get { return _onDeath; } set { _onDeath = value; } }
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        OnStart += SubscribeActions;
    }
    void SubscribeActions()
    {
        _hpBehaviour = ObjControl.BehaviourManager.Behaviours
            .Find(behaviour =>
            behaviour.GetType() == typeof(GiveHPSupportObjBehaviour)) as GiveHPSupportObjBehaviour;
        if (_hpBehaviour == null)
            return;

        if (_onDamage != null)
            _hpBehaviour.OnDamage += _onDamage;

        if (_onHeal != null)
            _hpBehaviour.OnHeal += _onHeal;

        if (_onDeath != null)
            _hpBehaviour.OnDeath += _onDeath;
    }

}
