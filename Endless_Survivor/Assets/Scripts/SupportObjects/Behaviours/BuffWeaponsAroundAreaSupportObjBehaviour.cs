using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BuffWeaponsAroundAreaSupportObjBehaviour : UseAreaAroundSupportObjBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField]WeaponStats _statBuffOnEnter;
    [SerializeField] float _buffDurationAfterLeavingArea;
    List<WeaponBuffHandler> _activeBuffs = new();
    List<GameObject> _weaponsInArea = new List<GameObject>();
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var buffWeaponsOriginal = original as BuffWeaponsAroundAreaSupportObjBehaviour;
        _statBuffOnEnter = buffWeaponsOriginal._statBuffOnEnter;
        _buffDurationAfterLeavingArea = buffWeaponsOriginal._buffDurationAfterLeavingArea;
        OnObjEnterArea += CheckIncomingObject;
        OnObjExitArea += DebuffExitingWeapon;
        OnObjExitArea += UnregisterExitingWeapon;

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
        WeaponBuffHandler buffHandler = new WeaponBuffHandler(new List<WeaponAttackManager> { weaponAttackManager }, _statBuffOnEnter, WeaponBuffHandler.BuffDurationType.WaitForExternal, 0, 0);
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
