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

    static WaveManager instance;

    public static WaveManager wm {  get { return instance; } }

    public List<GameObject> Enemies {  get { return _enemies; } }
    public int CurrWave{  get { return _currWave; } }

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

        Dictionary<dynamic, int> possibleEnemies = new Dictionary<dynamic, int>();
        foreach(var waveEnemy in newWave.WaveEnemies)
            possibleEnemies.Add(waveEnemy, waveEnemy.EnemyPoolWeight);

        Roulette enemySelectRoulette = new Roulette(possibleEnemies);

        float enemyCount = Random.Range(newWave.MinEnemyCount, newWave.MaxEnemyCount);
        Dictionary<WaveEnemy, int> spawnedEnemies = new Dictionary<WaveEnemy, int>();
        foreach (var waveEnemy in newWave.WaveEnemies)
            spawnedEnemies.Add(waveEnemy, 0);

        for(int i = 0; i < enemyCount; i++)
        {
            Vector2 enemyPosition = GetEnemyPosition();
            WaveEnemy spawnedEnemy = enemySelectRoulette.Spin();
            EnemyData enemyData = spawnedEnemy.EnemyData;
            _enemies.Add(SpawnEnemy(enemyData, enemyPosition));
            spawnedEnemies[spawnedEnemy]++;
        }

        List<WaveEnemy> notEnoughEnemies = spawnedEnemies.Where(spawnedEnemy => spawnedEnemy.Key.MinSpawnCount < spawnedEnemy.Value).Select(spawnedEnemy => spawnedEnemy.Key).ToList();
        foreach(var spawnedEnemy in notEnoughEnemies)
            while(spawnedEnemy.MinSpawnCount > spawnedEnemies[spawnedEnemy])
            {
                Vector2 enemyPosition = GetEnemyPosition();
                _enemies.Add(SpawnEnemy(spawnedEnemy.EnemyData, enemyPosition));
                spawnedEnemies[spawnedEnemy]++;
            }
        _currWave++;
        
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
