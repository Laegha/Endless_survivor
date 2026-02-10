using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyInvoker : MonoBehaviour
{
    [SerializeField] CustomAnimator _animator;
    [SerializeField] FilledBarRenderer _invokingBar;
    bool _fightingInvokedEnemy = false;
    List<EnemyInvokationPriority> _spawningEnemies = new();
    Action<EnemyControl> _onEnemyDeath;
    const float _spawnTime = 3;
    float _timer;
    public CustomAnimator Animator { get { return _animator; } }
    public Action<EnemyControl> OnEnemyDeath { get { return _onEnemyDeath;} set { _onEnemyDeath = value; } }

    public void AddInvokationEnemy(EnemyData enemy, int priority) =>_spawningEnemies.Add(new(enemy, priority));

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_fightingInvokedEnemy || _spawningEnemies.Count == 0) 
            return;
        _invokingBar.gameObject.SetActive(true);
        Time.timeScale -= .5f;
        _timer = _spawnTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_fightingInvokedEnemy || _spawningEnemies.Count == 0)
            return;
        Debug.Log(1 - (_timer / _spawnTime));
        _timer -= Time.deltaTime;
        _invokingBar.SetFillValue(1 - (_timer / _spawnTime), 1);
        if (_timer > 0)
            return;

        SpawnEnemy();

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (_fightingInvokedEnemy || _spawningEnemies.Count == 0)
            return;
        _invokingBar.gameObject.SetActive(false);
        Time.timeScale += .5f;
        
    }

    void SpawnEnemy()
    {
        _spawningEnemies.Sort((a, b) => b.priority.CompareTo(a.priority));
        EnemyData spawningEnemy = _spawningEnemies[0].enemy;
        var spawnTile = EnemySpawnManager.esm.GetEnemyPosition();
        var spawnedEnemy = EnemySpawnManager.esm.SpawnEnemy(spawnTile, spawningEnemy);

        var enemyHP = spawnedEnemy.GetComponent<EnemyControl>().EnemyHP;
        enemyHP.OnDeath += (enemyControl) => _fightingInvokedEnemy = false;
        spawnedEnemy.GetComponent<EnemyControl>().EnemyHP.OnDeath += _onEnemyDeath;

        _spawningEnemies.RemoveAt(0);
        Time.timeScale += .5f;
        _fightingInvokedEnemy = true;
    }
}

[Serializable]
class EnemyInvokationPriority
{
    public EnemyData enemy;
    [Tooltip("The higher the number, the higher priority")]public int priority;

    public EnemyInvokationPriority(EnemyData enemy, int priority)
    {
        this.enemy = enemy;
        this.priority = priority;
    }
}
