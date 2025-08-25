using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveEnemy
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] int _minSpawnCount;
    [SerializeField] int _enemyPoolWeight;

    public EnemyData EnemyData { get { return enemyData; } }
    public int MinSpawnCount { get { return _minSpawnCount; } }
    public int EnemyPoolWeight { get { return _enemyPoolWeight; } }
}
