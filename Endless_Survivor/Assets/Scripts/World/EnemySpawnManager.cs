using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor.Rendering;
using UnityEngine;

public class EnemySpawnManager : MonoBehaviour
{
    static readonly int _enemyTargetPriority = 0;
    Transform _player;
    List<GameObject> _enemies = new List<GameObject>();
    float _enemySpawnTimer = 0.5f;
    
    System.Action<EnemyControl> _onEnemySpawned;


    static EnemySpawnManager instance;

    public static EnemySpawnManager esm {  get { return instance; } }

    public List<GameObject> Enemies {  get { return _enemies; } }
    public System.Action<EnemyControl> OnEnemySpawned { get { return _onEnemySpawned; } set { _onEnemySpawned = value; } }

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
        if (_enemies.Count > GameManager.gm.WorldConfig.MaxConcurrentEnemies)
            return;
        _enemySpawnTimer -= Time.deltaTime;
        if (_enemySpawnTimer > 0)
            return;
        _enemySpawnTimer = GameManager.gm.WorldConfig.TimeBetweenEnemySpawn.rand;
        int spawnedEnemyCount = (int)GameManager.gm.WorldConfig.EnemiesPerSpawn.rand;
        for(int i = 0; i < spawnedEnemyCount; i++)
        {
            Tile spawingTile = GetEnemyPosition();
            EnemyData spawningEnemy = spawingTile.TileBiome.GetRandomAvailableEnemy(); ;
            SpawnEnemy(spawingTile, spawningEnemy);
        }
    }

    public Tile GetEnemyPosition()
    {
        var tilesInSpawnRange = MapManager.mm.LoadedTiles.Where(tile => CheckTileInRange(tile)).ToList();
        Tile spawningPos = tilesInSpawnRange[Random.Range(0, tilesInSpawnRange.Count)];
        return spawningPos;
    }

    bool CheckTileInRange(Tile tile)
    {
        Vector2 tileToPlayer = tile.transform.position - PlayerControl.pc.transform.position;
        if(tile.IsWall || tileToPlayer.magnitude > GameManager.gm.WorldConfig.MaxEnemySpawnDist || tileToPlayer.magnitude < GameManager.gm.WorldConfig.MinEnemySpawnDist)
            return false;
        return true;
    }

    public GameObject SpawnEnemy(Tile enemySpawnTile, EnemyData enemyData)
    {
        GameObject enemy = Instantiate(GameManager.gm.prefabHolder.Prefabs["Enemy"], enemySpawnTile.transform.position, Quaternion.identity);
        enemyData.TransferEnemyData(enemy);
        EnemyControl enemyControl = enemy.GetComponent<EnemyControl>();
        _onEnemySpawned?.Invoke(enemyControl);
        enemyControl.EnemyHP.OnDeath += EnemyKilled;
        _enemies.Add(enemy);
        WeaponAim.SharedAttackTargets.Add(new(enemy, _enemyTargetPriority));
        return enemy;
    }

    public void EnemyKilled(EnemyControl enemy)
    {
        _enemies.Remove(enemy.gameObject);
        WeaponAim.SharedAttackTargets.Remove(WeaponAim.SharedAttackTargets.Find(x => x.obj == enemy.gameObject));
        float enemyMaxHP = enemy.EnemyHP.MaxHP;
        IntensityManager.im.IncreaseIntensityProgress(ScalingFunctions.EnemyKillIntensityProgress(enemyMaxHP));

    }
}
