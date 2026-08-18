using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffWeaponsAroundAreaSupportObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => -1;
    [Tooltip("Here, wait for external means it will be debuffed at a given time after the player leaves the area")][SerializeField] WeaponBuffData _buffData;
    [SerializeField] float _buffDurationAfterLeavingArea;
    bool _playerInArea;
    bool _endedBuff = true;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var buffWeaponsOriginal = original as BuffWeaponsAroundAreaSupportObjBehaviour;
        _buffData = buffWeaponsOriginal._buffData;
        _buffDurationAfterLeavingArea = buffWeaponsOriginal._buffDurationAfterLeavingArea;
        OnObjEnterArea += CheckIncomingObject;
        if(_buffData.DurationType == WeaponBuffHandler.BuffDurationType.WaitForExternal)
            OnObjExitArea += DebuffExitingWeapon;

    }
    void CheckIncomingObject(GameObject obj)
    {
        if (obj.transform != PlayerControl.pc.transform)
            return;
        if(_endedBuff)
            WeaponBuffsManager.wbm.AddBuffStack(_buffData);
        _playerInArea = true;
        _endedBuff = false;
    }

    void DebuffExitingWeapon(GameObject exitingObject)
    {
        if (exitingObject.transform != PlayerControl.pc.transform)
            return;
        _playerInArea = false;
        GameManager.gm.DelayAction(_buffDurationAfterLeavingArea, EndBuff, () => _playerInArea);
    }
    void EndBuff()
    {
        WeaponBuffsManager.wbm.RemoveBuffStack(_buffData);
        _endedBuff = true;
    }
}
