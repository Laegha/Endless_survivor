using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New Wave", menuName = "ScriptableObjects/Wave", order = 5)]
public class Wave : ScriptableObject
{
    [SerializeField] List<WaveEnemy> _waveEnemies;
    [SerializeField] int _minEnemyCount;
    [SerializeField] int _maxEnemyCount;

    public List<WaveEnemy> WaveEnemies {  get { return _waveEnemies; } }
    public int MinEnemyCount { get { return _minEnemyCount; } }
    public int MaxEnemyCount { get { return _maxEnemyCount; } }
}
