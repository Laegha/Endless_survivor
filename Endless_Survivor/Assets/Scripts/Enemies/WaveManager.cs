using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] Wave[] _waves;
    [SerializeField] Transform[] _spawnBounds;
    [SerializeField] Vector2 _minSpawnDistFromPlayer;
    Transform _player;
    List<GameObject> _enemies = new List<GameObject>();
    int _currWave = 0;
    System.Action<EnemyControl> _onEnemySpawned;
    System.Action _onWaveStarted;

    static WaveManager instance;

    public static WaveManager wm {  get { return instance; } }

    public Transform[] SpawnBounds { get { return _spawnBounds; } }
    public List<GameObject> Enemies {  get { return _enemies; } }
    public int CurrWave{  get { return _currWave; } }
    public System.Action<EnemyControl> OnEnemySpawned { get { return _onEnemySpawned; } set { _onEnemySpawned = value; } }
    public System.Action OnWaveStarted { get { return _onWaveStarted; } set { _onWaveStarted = value; } }

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        StartCoroutine(GetPlayer());
    }

    IEnumerator GetPlayer()
    {
        while(_player == null)
        {
            _player = GameObject.FindWithTag("Player").transform;
            yield return null;
        }

        StartWave();
    }

    void StartWave()
    {
        Wave newWave;
        if(_currWave >= _waves.Length)
            newWave = _waves[_waves.Length - 1];
        else
            newWave = _waves[_currWave];

        Dictionary<WaveEnemy, int> possibleEnemies = new Dictionary<WaveEnemy, int>();
        foreach(var waveEnemy in newWave.WaveEnemies)
            possibleEnemies.Add(waveEnemy, waveEnemy.EnemyPoolWeight);

        Roulette<WaveEnemy> enemySelectRoulette = new Roulette<WaveEnemy>(possibleEnemies);

        float enemyCount = newWave.EnemyCount.rand;

        Dictionary<WaveEnemy, int> spawningEnemiesCounts = new Dictionary<WaveEnemy, int>();
        foreach (var waveEnemy in newWave.WaveEnemies)
            spawningEnemiesCounts.Add(waveEnemy, 0);

        for(int i = 0; i < enemyCount; i++)
        {
            //Vector2 enemyPosition = GetEnemyPosition();
            WaveEnemy spawnedEnemy = enemySelectRoulette.Spin();
            //_enemies.Add(SpawnEnemy(enemyData, enemyPosition));
            spawningEnemiesCounts[spawnedEnemy]++;
        }

        List<WaveEnemy> notEnoughEnemies = spawningEnemiesCounts.Where(spawnedEnemy => spawnedEnemy.Key.MinSpawnCount > spawnedEnemy.Value).Select(spawnedEnemy => spawnedEnemy.Key).ToList();
        foreach(var spawnedEnemy in notEnoughEnemies)
        {
            while(spawnedEnemy.MinSpawnCount > spawningEnemiesCounts[spawnedEnemy])
            {
                //Vector2 enemyPosition = GetEnemyPosition();
                //_enemies.Add(SpawnEnemy(spawnedEnemy.EnemyData, enemyPosition));
                spawningEnemiesCounts[spawnedEnemy]++;
            }

        }
        List<EnemyData> spawningEnemies = new();
        foreach(var enemy in spawningEnemiesCounts)
        {
            spawningEnemies.AddRange(Enumerable.Repeat(enemy.Key.EnemyData, enemy.Value));
        }

        StartCoroutine(WaveLoop(spawningEnemies, newWave));
        _currWave++;
        GameUIManager.uiManager.WaveDisplay.text = "Wave: " + _currWave;
        _onWaveStarted?.Invoke();
    }
    
    IEnumerator WaveLoop(List<EnemyData> spawningEnemies, Wave waveData)
    {
        float nextSpawnTimer = 0;
        while(spawningEnemies.Count > 0)
        {
            nextSpawnTimer -= Time.deltaTime;
            if(nextSpawnTimer <= 0)
            {
                nextSpawnTimer = waveData.TimeBetweenSpawns.rand;
                int spawnEnemyCount = Mathf.Clamp((int)waveData.EnemiesPerSpawn.rand, 1, spawningEnemies.Count);
                List<EnemyData> currSpawnEnemies = new();
                for(int i = 0; i < spawnEnemyCount; i++)
                {
                    var enemy = spawningEnemies[Random.Range(0, spawningEnemies.Count)];
                    currSpawnEnemies.Add(enemy);
                    spawningEnemies.Remove(enemy);
                }
                SpawnEnemies(currSpawnEnemies);
            }
            yield return null;
        }
    }
    void SpawnEnemies(List<EnemyData> spawningEnemies)
    {
        foreach(var enemy in spawningEnemies)
        {
            Vector2 enemyPosition = GetEnemyPosition();
            _enemies.Add(SpawnEnemy(enemy, enemyPosition));
        }
    }

    Vector2 GetEnemyPosition()
    {
        float xMax = Mathf.Max(_spawnBounds[0].position.x, _spawnBounds[1].position.x);
        float xMin = Mathf.Min(_spawnBounds[0].position.x, _spawnBounds[1].position.x);
        float yMax = Mathf.Max(_spawnBounds[0].position.y, _spawnBounds[1].position.y);
        float yMin = Mathf.Min(_spawnBounds[0].position.y, _spawnBounds[1].position.y);

        float xPos = Random.Range(xMin, xMax);
        float yPos = Random.Range(yMin, yMax);

        while(Mathf.Abs(xPos - _player.position.x) < _minSpawnDistFromPlayer.x)
            xPos = Random.Range(xMin, xMax);

        while (Mathf.Abs(yPos - _player.position.y)< _minSpawnDistFromPlayer.y)
            yPos = Random.Range(yMin, yMax);

        return new Vector2(xPos, yPos);
    }

    GameObject SpawnEnemy(EnemyData enemyData, Vector2 enemyPosition)
    {
        GameObject enemy = Instantiate(GameManager.gm.prefabHolder.Prefabs["Enemy"], enemyPosition, Quaternion.identity);
        enemyData.TransferEnemyData(enemy);
        _onEnemySpawned?.Invoke(enemy.GetComponent<EnemyControl>());
        return enemy;
    }

    public void EnemyKilled(GameObject enemy)
    {
        _enemies.Remove(enemy);
        Destroy(enemy);
        if (_enemies.Count == 0)
            StartWave();
    }
}
