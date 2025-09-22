using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupportObjectBehaviour
{
    public static bool isUsable = false;
    SupportObjectBehaviourManager _behaviourManager;

    Action _onStart;
    Action _onUpdate;
    Action _onCollidedWithPlayer;
    Action<EnemyControl> _onCollidedWithEnemy;
    Action _onCollidedWithOther;

    public SupportObjectBehaviourManager BehaviourManager { get { return _behaviourManager; } }
    public List<EnemyControl> closestEnemies => Utility.GetClosestTo(WaveManager.wm.Enemies, _behaviourManager.transform).ConvertAll(new Converter<GameObject, EnemyControl>((enemy) => enemy.GetComponent<EnemyControl>()));
    public Vector2 MapMinBound
    {
        get
        {
            var bounds = WaveManager.wm.SpawnBounds;
            Vector2 min = new Vector2(Mathf.Min(bounds[0].position.x, bounds[1].position.x), Mathf.Min(bounds[0].position.y, bounds[1].position.y));
            return min;
        }
    }
    public Vector2 MapMaxBound
    {
        get
        {
            var bounds = WaveManager.wm.SpawnBounds;
            Vector2 max = new Vector2(Mathf.Max(bounds[0].position.x, bounds[1].position.x), Mathf.Max(bounds[0].position.y, bounds[1].position.y));
            return max;
        }
    }
    public Action OnStart {  get { return _onStart; } set { _onStart = value; } }
    public Action OnUpdate { get { return _onUpdate; } set { _onUpdate = value; } }
    public Action OnCollidedWithPlayer { get { return _onCollidedWithPlayer; } set { _onCollidedWithPlayer = value; } }
    public Action<EnemyControl> OnCollidedWithEnemy { get { return _onCollidedWithEnemy;} set { _onCollidedWithEnemy = value; } }
    public Action OnCollidedWithOther { get { return _onCollidedWithOther; } set { _onCollidedWithOther = value; } }
    public virtual void Initiate(SupportObjectBehaviourManager manager, SupportObjectBehaviour original)
    {
        _behaviourManager = manager;
    }
}
