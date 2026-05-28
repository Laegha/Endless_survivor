using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyWeaponAfterAttacksAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _attacksUntilDestroyed;
    static Dictionary<WeaponControl, int> _attackCounters = new();
    DestroyWeaponAfterAttacksAttackEffect(DestroyWeaponAfterAttacksAttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    static List<Attack> _attackBuffer = new List<Attack>();//this bs is to prevent multiple effects from counting the same attack multiple times
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        if (!_attackCounters.ContainsKey(affectedAttack.ParentWeapon.WeaponControl))
            _attackCounters.Add(AffectedAttack.ParentWeapon.WeaponControl, _attacksUntilDestroyed);
        if (_attackCounters[affectedAttack.ParentWeapon.WeaponControl] > _attacksUntilDestroyed)
            _attackCounters[affectedAttack.ParentWeapon.WeaponControl] = _attacksUntilDestroyed;
        OnAttack += CountAttack;
    }

    void CountAttack()
    {
        if (_attackBuffer.Contains(AffectedAttack))
            return;
        var previousAttack = _attackBuffer.Find(x => x.ParentWeapon == AffectedAttack.ParentWeapon);
        if (previousAttack != null)
            _attackBuffer.Remove(previousAttack);
        _attackBuffer.Add(AffectedAttack);
        _attackCounters[AffectedAttack.ParentWeapon.WeaponControl] --;
        if(_attackCounters[AffectedAttack.ParentWeapon.WeaponControl] <= 0)
        {
            PlayerControl.pc.WeaponManager.RemoveWeapon(AffectedAttack.ParentWeapon.WeaponControl.WeaponAttackManager);
        }
    }
}