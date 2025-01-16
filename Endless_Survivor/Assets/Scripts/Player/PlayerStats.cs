using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    int _damage;
    int _attackSpeed;
    int _range;
    int _maxHealth;
    int _speed;

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
}
