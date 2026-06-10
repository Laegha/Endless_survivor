using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GiveHPSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] int _objMaxHp;
    HP _supportObjHP;
    Action<int> _onDamage;
    Action _onHeal;
    Action _onDeath;
    public HP SupportObjHP { get { return _supportObjHP; } }
    public Action<int> OnDamage { get {  return _onDamage; } set { _onDamage = value; } }
    public Action OnHeal { get {  return _onHeal; } set { _onHeal= value; } }
    public Action OnDeath {  get { return _onDeath; } set { _onDeath = value; } }
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var giveHPOriginal = original as GiveHPSupportObjBehaviour;
        _objMaxHp = giveHPOriginal._objMaxHp;
        _supportObjHP = ObjControl.AddComponent<HP>();
        _supportObjHP.InitializeHP(_objMaxHp);
        OnStart += () => GameManager.gm.DelayActionAFrame(SubscribeActionsToHP, null);
    }

    void SubscribeActionsToHP()
    {
        _supportObjHP.OnDamageTaken += _onDamage;
        _supportObjHP.OnDamageTaken += CheckDeath;
        _supportObjHP.OnHealed += _onHeal;
        
    }

    void CheckDeath(int _)
    {
        if (_supportObjHP.RemainingHP > 0)
            return;
        _onDeath?.Invoke();
        DestroyObj();
    }
}
