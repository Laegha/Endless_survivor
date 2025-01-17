using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WeaponStats
{
    [SerializeField] float _range;
    [SerializeField] float _attackSpeed;
    [SerializeField] int _damage;

    public float Range { get { return _range; } }
    public float AttackSpeed { get { return _attackSpeed; } }
    public int Damage { get { return _damage; } }

    public WeaponStats(WeaponStats original = null)
    {
        if (original == null)
            return;
        _range = original.Range;
        _attackSpeed = original.AttackSpeed;
        _damage = original.Damage;
    }
}
