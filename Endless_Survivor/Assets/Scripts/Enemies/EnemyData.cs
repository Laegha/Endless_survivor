using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Enemy", menuName = "ScriptableObjects/Enemy", order = 3)]
public class EnemyData : ScriptableObject
{
    [SerializeField]int _initialHP;
    [SerializeField] GameObject _enemyPrefab;

    public int InitialHP { get { return _initialHP; } }
    public GameObject EnemyPrefab { get { return _enemyPrefab; } }
}
