using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WaveManager : MonoBehaviour
{
    [SerializeField] Transform[] _spawnBounds;
    [SerializeField] Vector2 _minSpawnDistFromPlayer;
    Transform _player;
    List<GameObject> _enemies = new List<GameObject>();
    int _lapsedWaves;

    static WaveManager instance;

    public static WaveManager wm {  get { return instance; } }

    public List<GameObject> Enemies {  get { return _enemies; } }

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
        _lapsedWaves++;
        float enemyCount = 3 + Mathf.Clamp(Random.Range(_lapsedWaves, _lapsedWaves), 0, Mathf.Infinity);
        for(int i = 0; i < enemyCount; i++)
        {
            Vector2 enemyPosition = GetEnemyPosition();
            GameObject enemy = Instantiate(GameManager.gm.enemiesPrefabs[Random.Range(0, GameManager.gm.enemiesPrefabs.Length)], enemyPosition, Quaternion.identity);
            _enemies.Add(enemy);
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

    public void EnemyKilled(GameObject enemy)
    {
        _enemies.Remove(enemy);
        Destroy(enemy);
        if (_enemies.Count == 0)
            StartWave();
    }
}
