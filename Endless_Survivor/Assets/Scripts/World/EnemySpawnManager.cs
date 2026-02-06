using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    [SerializeField] float _minSpawnDistFromPlayer;
    [SerializeField] float _maxSpawnDistFromPlayer;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenEnemySpawns;
    [SerializeField] RandomBetweenTwoConstants _enemiesPerSpawn;
    Transform _player;
    List<GameObject> _enemies = new List<GameObject>();
    float _enemySpawnTimer = 0.5f;
    
    System.Action<EnemyControl> _onEnemySpawned;
    System.Action _onWaveStarted;


    static EnemySpawnManager instance;

    public static EnemySpawnManager esm {  get { return instance; } }

    public List<GameObject> Enemies {  get { return _enemies; } }
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

    }
    private void Update()
    {
        _enemySpawnTimer -= Time.deltaTime;
        if (_enemySpawnTimer > 0)
            return;
        _enemySpawnTimer = _timeBetweenEnemySpawns.rand;
        int spawnedEnemyCount = (int)_enemiesPerSpawn.rand;
        for(int i = 0; i < spawnedEnemyCount; i++)
        {
            Tile spawingTile = GetEnemyPosition();
            _enemies.Add(SpawnEnemy(spawingTile));
        }

    }

    Tile GetEnemyPosition()
    {
        var tilesInSpawnRange = MapManager.mm.LoadedTiles.Where(tile => CheckTileInRange(tile)).ToList();
        Tile spawningPos = tilesInSpawnRange[Random.Range(0, tilesInSpawnRange.Count)];
        return spawningPos;
    }

    bool CheckTileInRange(Tile tile)
    {
        Vector2 tileToPlayer = tile.transform.position - PlayerControl.pc.transform.position;
        if(tile.IsWall || tileToPlayer.magnitude > _maxSpawnDistFromPlayer || tileToPlayer.magnitude < _minSpawnDistFromPlayer)
            return false;
        return true;
    }

    GameObject SpawnEnemy(Tile enemySpawnTile)
    {
        EnemyData enemyData = enemySpawnTile.TileBiome.GetRandomAvailableEnemy();
        GameObject enemy = Instantiate(GameManager.gm.prefabHolder.Prefabs["Enemy"], enemySpawnTile.transform.position, Quaternion.identity);
        enemyData.TransferEnemyData(enemy);
        EnemyControl enemyControl = enemy.GetComponent<EnemyControl>();
        _onEnemySpawned?.Invoke(enemyControl);
        enemyControl.EnemyHP.OnDeath += EnemyKilled;
        return enemy;
    }

    public void EnemyKilled(EnemyControl enemy)
    {
        _enemies.Remove(enemy.gameObject);
        float enemyMaxHP = enemy.EnemyHP.MaxHP;
        IntensityManager.im.IncreaseIntensityProgress(ScalingFunctions.EnemyKillIntensityProgress(enemyMaxHP));

    }
}
