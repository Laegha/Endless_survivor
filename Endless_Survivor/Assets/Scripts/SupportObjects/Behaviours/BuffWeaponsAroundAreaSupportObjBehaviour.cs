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
    List<WeaponBuffHandler> _activeBuffs = new();
    List<GameObject> _weaponsInArea = new List<GameObject>();
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var buffWeaponsOriginal = original as BuffWeaponsAroundAreaSupportObjBehaviour;
        _buffData = buffWeaponsOriginal._buffData;
        _buffDurationAfterLeavingArea = buffWeaponsOriginal._buffDurationAfterLeavingArea;
        OnObjEnterArea += CheckIncomingObject;
        OnObjExitArea += UnregisterExitingWeapon;
        if(_buffData.DurationType == WeaponBuffHandler.BuffDurationType.WaitForExternal)
            OnObjExitArea += DebuffExitingWeapon;

    }

    void UnregisterExitingWeapon(GameObject exitingObj)
    {
        if(_weaponsInArea.Contains(exitingObj))
            _weaponsInArea.Remove(exitingObj);
    }

    void CheckIncomingObject(GameObject obj)
    {
        WeaponControl weapon = obj.transform.root.GetComponent<WeaponControl>();
        if (weapon == null)
            return;
        _weaponsInArea.Add(weapon.gameObject);
        BuffWeapon(weapon.WeaponAttackManager);
    }

    void BuffWeapon(WeaponAttackManager weaponAttackManager)
    {
        if (GetRelatedBuff(weaponAttackManager) != null)
            return;
        WeaponBuffHandler buffHandler = new WeaponBuffHandler(new List<WeaponAttackManager> { weaponAttackManager }, _buffData);
        _activeBuffs.Add(buffHandler);
    }

    void DebuffExitingWeapon(GameObject exitingObject)
    {
        WeaponControl weapon = exitingObject.transform.root.GetComponent<WeaponControl>();
        if (weapon == null)
            return;
        WeaponBuffHandler relatedBuff = GetRelatedBuff(weapon.WeaponAttackManager);
        //delay this shit
        Action delayedDebuff = null;
        delayedDebuff += relatedBuff.DebuffWeapons;
        delayedDebuff += () => _activeBuffs.Remove(relatedBuff);
        GameManager.gm.DelayAction(_buffDurationAfterLeavingArea, delayedDebuff, () =>_weaponsInArea.Contains(exitingObject));
    }

    WeaponBuffHandler GetRelatedBuff(WeaponAttackManager weapon)
    {
        WeaponBuffHandler relatedBuff = _activeBuffs.Find(x => x.buffedWeapons.Contains(weapon));
        return relatedBuff;

    }
}
