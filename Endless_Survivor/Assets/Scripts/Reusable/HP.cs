using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HP : MonoBehaviour
{
    int _remainingHP;
    int _maxHP;
    public int RemainingHP {  get { return _remainingHP; } set { _remainingHP = value; } }
    public int MaxHP {  get { return _maxHP; } set { _maxHP = value; } }
    public void InitializeHP(int maxHP)
    {
        MaxHP = maxHP;
        RemainingHP = maxHP;
    }
    public virtual void TakeDamage(int incomingDamage) 
    {
        _remainingHP -= incomingDamage;
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
