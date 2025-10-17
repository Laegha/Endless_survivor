using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    int _remainingHP;
    int _maxHP;
    float _incomingDamageMultiplier = 1;
    Action _onDamageTaken;
    Action _onHealed;
    public int RemainingHP {  get { return _remainingHP; } set { _remainingHP = value; } }
    public int MaxHP {  get { return _maxHP; } set { _maxHP = value; } }
    public Action OnDamageTaken { get { return _onDamageTaken; } set { _onDamageTaken = value; } }
    public Action OnHealed { get { return _onHealed; } set { _onHealed  = value; } }
    public float IncomingDamageMultiplier 
    {
        get { return _incomingDamageMultiplier > 0.1f ? _incomingDamageMultiplier : 0.1f; }
        set 
        {
            _incomingDamageMultiplier = value;
        } 
    }
    public void InitializeHP(int maxHP)
    {
        MaxHP = maxHP;
        RemainingHP = maxHP;
    }
    public virtual void Heal(int healedHP)
    {
        _remainingHP = Mathf.Clamp(_remainingHP + healedHP, 0, _maxHP);
        _onHealed?.Invoke();
    }
    public virtual void TakeDamage(int incomingDamage) 
    {
        _onDamageTaken?.Invoke();
        _remainingHP -= (int)(incomingDamage * _incomingDamageMultiplier);
        if (_remainingHP <= 0)
        {
            Die();
            return;
        }
    }
    public virtual void Die()
    {

    }
}
