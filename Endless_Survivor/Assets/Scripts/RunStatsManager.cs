using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStatsManager : MonoBehaviour
{
    public static RunStatsManager instance;
    public static RunStatsManager runStatsManager {  get { return instance; } }

    [HideInInspector]public int wavesSurvived;
    [HideInInspector]public int totalDamageDealt;
    [HideInInspector]public int regularEnemiesKilled;
    [HideInInspector]public int minibossesKilled;

    private void Awake()
    {
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        EnemySpawnManager.esm.OnWaveStarted += IncreaseWaveCounter;
    }

    public void DamageDealt(int damage)
    {
        totalDamageDealt += damage;
    }
    public void EnemyKilled(EnemyControl killedEnemy)
    {
        regularEnemiesKilled++;
    }
    void IncreaseWaveCounter()
    {
        wavesSurvived++;
    }
}
