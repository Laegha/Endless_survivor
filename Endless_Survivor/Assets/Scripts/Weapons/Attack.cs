using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    [SerializeField] AttackEffectsHandler _effectsHandler;
    Weapon _parentWeapon;
    int _attackDamage = 0;
    float _attackDamageMultiplier = 1;
    public AttackEffectsHandler EffectsHandler { get { return _effectsHandler; } }
    public int AttackDamage { get { return (int)(_attackDamage * _attackDamageMultiplier); } set { _attackDamage = value; } }
    public float AttackDamageMultiplier { get { return _attackDamageMultiplier; } set { _attackDamageMultiplier = value; } }
    public Weapon ParentWeapon { get { return _parentWeapon; } set { _parentWeapon = value; } }
}
