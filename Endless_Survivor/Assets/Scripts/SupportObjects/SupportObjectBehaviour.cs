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

    public List<EnemyControl> closestEnemies => Utility.GetClosestTo(WaveManager.wm.Enemies, _behaviourManager.transform).ConvertAll(new Converter<GameObject, EnemyControl>((enemy) => enemy.GetComponent<EnemyControl>()));
    public Action OnStart {  get { return _onStart; } }
    public Action OnUpdate { get { return _onUpdate; } }
    public Action OnCollidedWithPlayer { get { return _onCollidedWithPlayer; } }
    public Action<EnemyControl> OnCollidedWithEnemy { get { return _onCollidedWithEnemy;} }
    public virtual void Initiate(SupportObjectBehaviourManager manager, SupportObjectBehaviour original)
    {
        _behaviourManager = manager;
    }
}
