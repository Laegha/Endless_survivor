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
    Dictionary<EnemyControl, EnemyInvokationInfo> _spawnedEnemies = new(); 
    Action<EnemyControl> _onEnemyDeath;
    const float _spawnTime = 2f;
    const float _timescaleWhileInvoking = .5f;
    float _timer;
    TimescaleChangeInfo _currTimescaleChange;

    public CustomAnimator Animator { get { return _animator; } }
    public Action<EnemyControl> OnEnemyDeath { get { return _onEnemyDeath;} set { _onEnemyDeath = value; } }

    public void AddInvokationEnemy(EnemyInvokationInfo enemy, int priority)
    {
        if(enemy.IsBoss)
            GameProgressionManager.gpm.AddBoss();
        _spawningEnemies.Add(new(enemy, priority));
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (_fightingInvokedEnemy || _spawningEnemies.Count == 0) 
            return;
        _invokingBar.gameObject.SetActive(true);
        _currTimescaleChange = TimescaleManager.tm.AddTimescaleChange(new(_timescaleWhileInvoking, false, 0));
        _timer = _spawnTime;
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (_fightingInvokedEnemy || _spawningEnemies.Count == 0)
            return;
        _timer -= Time.unscaledDeltaTime;
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
        TimescaleManager.tm.RemoveTimescaleChange(_currTimescaleChange);
    }

    void SpawnEnemy()
    {
        _spawningEnemies.Sort((a, b) => a.priority.CompareTo(b.priority));
        EnemyData spawningEnemy = _spawningEnemies[0].enemyInvokationInfo.InvokedEnemy;
        var spawnTile = EnemySpawnManager.esm.GetEnemyPosition();
        var spawnedEnemy = EnemySpawnManager.esm.SpawnEnemy(spawnTile, spawningEnemy);

        var spawnedEnemyControl = spawnedEnemy.GetComponent<EnemyControl>();
        _spawnedEnemies.Add(spawnedEnemyControl, _spawningEnemies[0].enemyInvokationInfo);
        var enemyHP = spawnedEnemyControl.EnemyHP;
        enemyHP.OnDeath += InvokedEnemyKilled;
        spawnedEnemy.GetComponent<EnemyControl>().EnemyHP.OnDeath += _onEnemyDeath;

        var pointer = GameUIManager.uiManager.PointerManager.AddPointer(spawnedEnemy.transform, _spawningEnemies[0].enemyInvokationInfo.PointerColor, _spawningEnemies[0].enemyInvokationInfo.PointerIcon);
        enemyHP.OnDeath += (placeholder) => GameUIManager.uiManager.PointerManager.RemovePointer(pointer);

        _spawningEnemies.RemoveAt(0);
        TimescaleManager.tm.RemoveTimescaleChange(_currTimescaleChange);
        _invokingBar.gameObject.SetActive(false);
        _fightingInvokedEnemy = true;
    }
    void InvokedEnemyKilled(EnemyControl killedEnemy)
    {
        _fightingInvokedEnemy = false;
        if (_spawnedEnemies[killedEnemy].IsBoss)
            GameProgressionManager.gpm.BossKilled();
    }
}

[Serializable]
class EnemyInvokationPriority
{
    public EnemyInvokationInfo enemyInvokationInfo;
    [Tooltip("The higher the number, the higher priority")]public int priority;

    public EnemyInvokationPriority(EnemyInvokationInfo enemyInvokationInfo, int priority)
    {
        this.enemyInvokationInfo = enemyInvokationInfo;
        this.priority = priority;
    }
}
