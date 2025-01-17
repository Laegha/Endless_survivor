using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHP : MonoBehaviour
{
    [SerializeField] EnemyData _enemyData;
    int _leftHP;

    private void Start()
    {
        _leftHP = _enemyData.InitialHP + WaveManager.wm.LapsedWaves * 3;
    }

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
