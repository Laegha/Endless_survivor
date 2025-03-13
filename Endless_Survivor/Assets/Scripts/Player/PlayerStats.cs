using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    [SerializeField]int _damage = 3;
    [SerializeField]float _damageMultiplier = 1;
    [SerializeField]int _attackSpeed = 1;
    [SerializeField]int _range = 1;
    [SerializeField]int _maxHealth = 10;
    [SerializeField]int _speed = 1;

    public int Damage
    {
        get { return (int)(_damage * _damageMultiplier); } set { _damage = value;  }
    }
    public float DamageMultiplier
    {
        get { return _damageMultiplier; }
        set { _damageMultiplier = value; }
    }

    public int AttackSpeed
    {
        get { return _attackSpeed; } set { _attackSpeed = value; }
    }

    public int Range
    {
        get { return _range; } set { _range = value; }
    }

    public int MaxHealth
    {
        get { return _maxHealth; } set { _maxHealth = value; }
    }

    public int Speed
    {
        get { return _speed; } set { _speed = value; }
    }

    public PlayerStats(PlayerStats original = null)
    {
        if (original == null)
            return;

        Damage = original.Damage;
        AttackSpeed = original.AttackSpeed;
        Range = original.Range;
        MaxHealth = original.MaxHealth;
        Speed = original.Speed;
    }
}
