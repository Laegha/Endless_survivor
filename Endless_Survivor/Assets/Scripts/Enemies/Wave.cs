using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Wave", menuName = "ScriptableObjects/Wave", order = 5)]
public class Wave : ScriptableObject
{
    [SerializeField] List<WaveEnemy> _waveEnemies;
    [SerializeField] RandomBetweenTwoConstants _enemyCount;
    [SerializeField] RandomBetweenTwoConstants _enemiesPerSpawn;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenSpawns;

    public List<WaveEnemy> WaveEnemies {  get { return _waveEnemies; } }
    public RandomBetweenTwoConstants EnemyCount { get { return _enemyCount; } }
    public RandomBetweenTwoConstants EnemiesPerSpawn { get { return _enemiesPerSpawn; } }
    public RandomBetweenTwoConstants TimeBetweenSpawns { get { return _timeBetweenSpawns; } }
}
