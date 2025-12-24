using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    //[SerializeField]float _damageMultiplier = 1;
    [SerializeField] int _initialHP = 10;
    [SerializeField] int _hpIncrement = 10;
    [SerializeField] float _hpRegeneration = 1f;
    [SerializeField] float _speed = 1;
    [SerializeField] float _acceleration = 1f;

    //public float DamageMultiplier
    //{
    //    get { return _damageMultiplier; }
    //    set { _damageMultiplier = value; }
    //}

    public int InitialHP
    {
        get { return _initialHP; } set { _initialHP = value; }
    }
    public int HPIncrement
    {
        get { return _hpIncrement; } set { _hpIncrement = value; }
    }
    public float HPRegeneration
    {
        get { return _hpRegeneration; } set { _hpRegeneration = value; } 
    }
    public float Speed
    {
        get { return _speed; } set { _speed = value; }
    }
    public float Acceleration
    {
        get { return _acceleration; } set { _acceleration = value; }
    }

    public PlayerStats(PlayerStats original = null)
    {
        if (original == null)
            return;

        _initialHP = original._initialHP;
        _hpIncrement = original._hpIncrement;
        _hpRegeneration = original._hpRegeneration;
        _speed = original._speed;
        _acceleration = original._acceleration;
    }
}
