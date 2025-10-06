using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseAreaAroundSupportObjBehaviour : SupportObjectBehaviour
{
    new public static bool isUsable => false;
    [SerializeField] string _areaName = "AreaIdentifier";
    CheckAreaAroundSupportObjBehaviour _checkAreaBehaviour;

    Action<GameObject> _onObjEnterArea;
    Action<GameObject> _onObjUpdateArea;
    Action<GameObject> _onObjExitArea;
    //Sub-classes should subscribe to this on Initiate
    public Action<GameObject> OnObjEnterArea { get { return _onObjEnterArea; } set { _onObjEnterArea = value; } }
    public Action<GameObject> OnObjUpdateArea { get { return _onObjUpdateArea; } set { _onObjUpdateArea = value; } }
    public Action<GameObject> OnObjExitArea { get { return _onObjExitArea; } set { _onObjExitArea = value; } }

    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        _checkAreaBehaviour = control.BehaviourManager.Behaviours
            .Find(behaviour => 
            behaviour.GetType() == typeof(CheckAreaAroundSupportObjBehaviour) 
            && ((CheckAreaAroundSupportObjBehaviour)behaviour).AreaName == _areaName)
            as CheckAreaAroundSupportObjBehaviour;
        OnStart += SubscribeActions;
    }
    void SubscribeActions()
    {
        if (_checkAreaBehaviour == null)
            return;

        if (_onObjEnterArea != null)
            _checkAreaBehaviour.OnObjEnterArea += _onObjEnterArea;

        if (_onObjUpdateArea != null)
            _checkAreaBehaviour.OnObjUpdateArea += _onObjUpdateArea;

        if (_onObjExitArea != null)
            _checkAreaBehaviour.OnObjExitArea+= _onObjExitArea;
    }
}
