using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiveRerollsOnMenuItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;

    [SerializeField] int _usePriority;
    [SerializeField] int _rerollAmmount;
    [SerializeField] bool _onWeaponPickup;
    [SerializeField] bool _onPassivePickup;
    Reroll _reroll;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        GiveRerollsOnMenuItemBehaviour giveRollsEveryMenuOriginal = original as GiveRerollsOnMenuItemBehaviour;
        _usePriority = giveRollsEveryMenuOriginal._usePriority;
        _rerollAmmount = giveRollsEveryMenuOriginal._rerollAmmount;
        _reroll = new(_usePriority, _rerollAmmount);
        _onWeaponPickup = giveRollsEveryMenuOriginal._onWeaponPickup;
        _onPassivePickup = giveRollsEveryMenuOriginal._onPassivePickup;
        if(_onWeaponPickup)
            GameUIManager.uiManager.WeaponPickupMenu.OnMenuOpen += AddMissingRolls;
        if(_onPassivePickup)
            GameUIManager.uiManager.PassiveItemPickupMenu.OnMenuOpen += AddMissingRolls;

    }
    public void AddMissingRolls()
    {
        int usedRolls = _rerollAmmount - _reroll.rerollsLeft;
        _reroll.rerollsLeft += usedRolls;
        if (!RerollManager.rm.ContainsRoll(_reroll))
        {
            RerollManager.rm.AddReroll(_reroll);
        }
    }
    public override void RemoveBehaviour()
    {

        if (_onWeaponPickup)
            GameUIManager.uiManager.WeaponPickupMenu.OnMenuOpen -= AddMissingRolls;
        if (_onPassivePickup)
            GameUIManager.uiManager.PassiveItemPickupMenu.OnMenuOpen -= AddMissingRolls;
    }
}
