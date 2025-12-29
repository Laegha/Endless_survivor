using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPlayerOnEnemyHitItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] WeaponAttackControllerTypeHelper.AttackControllerTypes[] _weaponsThatHeal;
    [SerializeField] int _healAmmount;
    List<Type> _enlistedTypes = new();
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var healPlayerOriginal = original as HealPlayerOnEnemyHitItemBehaviour;
        _healAmmount = healPlayerOriginal._healAmmount;
        _weaponsThatHeal = healPlayerOriginal._weaponsThatHeal;
        foreach(var weaponType in _weaponsThatHeal)
        {
            _enlistedTypes.Add(WeaponAttackControllerTypeHelper.GetWeaponTypeFromEnum(weaponType));
        }
        behaviourManager.onEnemyHit += HealPlayer;
    }

    void HealPlayer(EnemyControl hitEnemy, Attack recievedAttack)
    {
        if (!_enlistedTypes.Contains(recievedAttack.ParentWeapon.GetType()))
            return;
        PlayerControl.pc.PlayerHPManager.Heal(_healAmmount);
    }

}
