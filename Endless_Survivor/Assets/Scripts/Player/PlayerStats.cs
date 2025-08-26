using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerStats
{
    //[SerializeField]float _damageMultiplier = 1;
    [SerializeField]int _initialHP = 10;
    [SerializeField]int _hpIncrement = 10;
    [SerializeField]int _speed = 1;

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
    public int Speed
    {
        get { return _speed; } set { _speed = value; }
    }

    public PlayerStats(PlayerStats original = null)
    {
        if (original == null)
            return;

        InitialHP = original.InitialHP;
        Speed = original.Speed;
    }
}
