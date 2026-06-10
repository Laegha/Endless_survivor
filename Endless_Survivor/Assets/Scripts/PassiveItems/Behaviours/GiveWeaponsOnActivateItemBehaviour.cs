using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveWeaponsOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<WeaponData> _addingWeapons;
    //[SerializeField] bool _removeWeaponsAfterTime;
    //[SerializeField] bool _removeWeaponsIfThisIsRemoved;
    //[SerializeField] float _timeToRemoveWeapons;

    //List<WeaponAttackManager> _addedWeapons;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var giveWeaponsOriginal = original as GiveWeaponsOnActivateItemBehaviour;
        _addingWeapons = giveWeaponsOriginal._addingWeapons;
        //_removeWeaponsAfterTime = giveWeaponsOriginal._removeWeaponsAfterTime;
        //_removeWeaponsIfThisIsRemoved = giveWeaponsOriginal._removeWeaponsIfThisIsRemoved;
        //_timeToRemoveWeapons = giveWeaponsOriginal._timeToRemoveWeapons;
    }

    public override void Activate()
    {
        base.Activate();
        AddWeapons();
    }

    void AddWeapons()
    {
        foreach (var weaponData in _addingWeapons)
        {
            var weaponStats = new WeaponStats(weaponData.WeaponStats);
            weaponStats.SetTrueLevelStats(weaponData.StatsIncreaseScale, ScalingFunctions.CurrScalingLevel);
            PlayerControl.pc.WeaponManager.PickupWeapon(weaponData, weaponStats);
        }
        //if (_removeWeaponsAfterTime)
            //GameManager.gm.DelayAction(_timeToRemoveWeapons, RemoveAnIterationOfWeapons, () => BehaviourManager == null);
    }
    //void RemoveAnIterationOfWeapons()
    //{
    //    List<PassiveItem> addedItemsCopy = new(_addedWeapons);
    //    for (int i = 0; i < _addingWeapons.Count; i++)
    //    {
    //        PlayerControl.pc.PassiveItemManager.RemovePassiveItem(addedItemsCopy[i]);
    //        _addedWeapons.RemoveAt(i);
    //    }
    //}
    //void RemoveAllWeapons()
    //{
    //    foreach (var weapon in _addedWeapons)
    //    {
    //        PlayerControl.pc.WeaponManager.RemoveWeapon(weapon);
    //    }
    //}
    public override void RemoveBehaviour()
    {
        //if (_removeWeaponsIfThisIsRemoved)
            //RemoveAllWeapons();
    }
}
