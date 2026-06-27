using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunStatsManager : MonoBehaviour
{
    public static RunStatsManager instance;
    public static RunStatsManager runStatsManager {  get { return instance; } }

    [HideInInspector]public int intensityLevelsSurvived;
    [HideInInspector]public int totalDamageDealt;
    [HideInInspector]public int regularEnemiesKilled;
    [HideInInspector]public int minibossesKilled;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        if (instance != null)
            Destroy(instance.gameObject);
        instance = this;
    }

    private void Start()
    {
        IntensityManager.im.OnLevelIncrease += IncreaseLevelCounter;
    }

    public void DamageDealt(int damage)
    {
        totalDamageDealt += damage;
    }
    public void EnemyKilled(EnemyControl killedEnemy)
    {
        regularEnemiesKilled++;
    }
    void IncreaseLevelCounter()
    {
        intensityLevelsSurvived++;
    }
}
