using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Attack : MonoBehaviour
{
    [SerializeField] AttackEffectsHandler _effectsHandler;
    WeaponAttackController _parentWeapon;
    int _attackDamage = 0;
    float _attackDamageMultiplier = 1;
    float _knockback;
    float _knockbackMultiplier = 1;
    int _attackPiercing = 0;
    bool _isSecondaryAttack;
    bool _triggersPassiveItemHit = true;
    public AttackGfxInterface AttackGfxInterface => new AttackGfxInterface();
    public abstract AttackEffectArea AttackEffectArea { get; }
    public AttackEffectsHandler EffectsHandler { get { return _effectsHandler; } }
    public int AttackDamage { get { return (int)(_attackDamage * _attackDamageMultiplier); } set { _attackDamage = value; } }
    public float AttackDamageMultiplier { get { return _attackDamageMultiplier; } set { _attackDamageMultiplier = value; } }
    public float AttackKnockback { get { return _knockback * _knockbackMultiplier; } set {  _knockback = value; } }
    public float AttackKnockbackMultiplier { get { return _knockbackMultiplier; } set { _knockbackMultiplier = value; } }
    public WeaponAttackController ParentWeapon { get { return _parentWeapon; } set { _parentWeapon = value; } }
    public int AttackPiercing { get { return _attackPiercing; } set { _attackPiercing = value; } }
    public bool IsSecondaryAttack { get { return _isSecondaryAttack; } set { _isSecondaryAttack = value; } }
    public bool TriggersPassiveItemHit { get { return _triggersPassiveItemHit; } set { _triggersPassiveItemHit = value; } }

    public abstract void ChangeGfx(AttackGfxInterface gfxInterface);
}
