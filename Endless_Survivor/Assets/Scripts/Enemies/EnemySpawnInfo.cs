using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class EnemySpawnInfo
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] int _spawnIntensity;

    public EnemyData EnemyData { get { return enemyData; } }
    public int SpawnIntensity { get { return _spawnIntensity; } }
}
