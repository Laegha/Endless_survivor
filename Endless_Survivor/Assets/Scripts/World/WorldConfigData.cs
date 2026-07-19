using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "New WorldConfig", menuName = "ScriptableObjects/World config", order = 0)]
public class WorldConfigData : ScriptableObject
{
    [SerializeField] float _minEnemySpawnDist;
    [SerializeField] float _maxEnemySpawnDist;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenEnemySpawn;
    [SerializeField] RandomBetweenTwoConstants _enemiesPerSpawn;
    [SerializeField] int _maxConcurrentEnemies;
    [SerializeField] int _initialIntensityGoal;
    [SerializeField] int _intensityGoalIncrease;
    [SerializeField] int _enemyHPPercentToIntensityIncrease;
    [SerializeField] int _intensityLevelsForNewBiome;
    [SerializeField] UIMessageInfo _intensityIncreaseMessageInfo;
    [InspectorLabel("Exit run info")]
    [SerializeField] RunExit _runExitPrefab;
    [SerializeField] UIMessageInfo _onExitCreatedMessageInfo;
    [SerializeField] Color _exitPointerColor;
    [SerializeField] Sprite _exitPointerIcon;
    public float MinEnemySpawnDist { get { return _minEnemySpawnDist; } }
    public float MaxEnemySpawnDist { get { return _maxEnemySpawnDist; } }
    public RandomBetweenTwoConstants TimeBetweenEnemySpawn { get { return _timeBetweenEnemySpawn; } }
    public RandomBetweenTwoConstants EnemiesPerSpawn { get { return _enemiesPerSpawn; } }
    public int MaxConcurrentEnemies { get { return _maxConcurrentEnemies; } }
    public int InitialInstensityGoal { get { return _initialIntensityGoal; } }
    public int IntensityGoalIncrease { get { return _intensityGoalIncrease; } }
    public int EnemyHPPercentToIntensityIncrease { get { return _enemyHPPercentToIntensityIncrease; } }
    public int IntensityLevelsForNewBiome { get { return _intensityLevelsForNewBiome; } }
    public UIMessageInfo IntensityIncreaseMessageInfo {  get { return _intensityIncreaseMessageInfo; } }
    public RunExit RunExitPrefab { get { return _runExitPrefab; } }
    public UIMessageInfo OnExitCreatedMessageInfo { get { return _onExitCreatedMessageInfo; } }
    public Color ExitPointerColor { get { return _exitPointerColor; } } 
    public Sprite ExitPointerIcon { get { return _exitPointerIcon; } } 
}
