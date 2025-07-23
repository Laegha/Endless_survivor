using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStatsManager : MonoBehaviour
{
    public static RunStatsManager instance;
    public static RunStatsManager runStatsManager {  get { return instance; } }

    public int wavesSurvived;
    public int totalDamageDealt;
    public int regularEnemiesKilled;
    public int minibossesKilled;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        WaveManager.wm.OnEnemySpawned += AddActionsToEnemy;
        WaveManager.wm.OnWaveStarted += IncreaseWaveCounter;
    }

    void DamageDealt(EnemyControl enemyControl, int damage)
    {
        totalDamageDealt += damage;
    }
    void EnemyKilled(EnemyControl killedEnemy)
    {
        regularEnemiesKilled++;
    }
    void AddActionsToEnemy(EnemyControl enemyControl)
    {
        enemyControl.EnemyHP.OnEnemyDamaged += DamageDealt;
        enemyControl.EnemyHP.OnEnemyDeath += EnemyKilled;
    }
    void IncreaseWaveCounter()
    {
        wavesSurvived++;
    }
}
