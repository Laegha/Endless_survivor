using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] EnemyData _enemyData;
    int _leftHP;

    public int LeftHP {  get { return _leftHP; } set { _leftHP = value; } }

    public void RecieveDamage(int incomingDamage)
    {
        _leftHP -= incomingDamage;
        if (_leftHP < 0)
            Die();

    }

    void Die()
    {
        WaveManager.wm.EnemyKilled(gameObject);
    }
}
