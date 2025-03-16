using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WaveEnemy
{
    [SerializeField] EnemyData enemyData;
    [SerializeField] int _minSpawnCount;
    [SerializeField] float _enemyPoolWeight;
    [SerializeField] float _dropChance;

    public EnemyData EnemyData { get { return enemyData; } }
}
