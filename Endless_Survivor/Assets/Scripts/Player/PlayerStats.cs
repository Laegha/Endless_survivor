using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    [SerializeField]int _damage;
    [SerializeField]int _attackSpeed;
    [SerializeField]int _range;
    [SerializeField]int _maxHealth;
    [SerializeField]int _speed;

    public int Damage
    {
        get { return _damage; } set { _damage = value; }
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
