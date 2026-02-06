using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SupportObjectBehaviour
{
    public static int maxStacks = 0;
    SupportObjectControl _supportObjControl;

    Action _onStart;
    Action _onUpdate;
    Action _onLateUpdate;
    Action _onCollidedWithPlayer;
    Action<EnemyControl> _onCollidedWithEnemy;
    Action _onCollidedWithOther;
    Action _onDestroyed;
    public SupportObjectControl ObjControl { get { return _supportObjControl; } }
    public List<EnemyControl> closestEnemies => Utility.GetClosestTo(EnemySpawnManager.esm.Enemies, _supportObjControl.transform).ConvertAll(new Converter<GameObject, EnemyControl>((enemy) => enemy.GetComponent<EnemyControl>()));
    public Action OnStart {  get { return _onStart; } set { _onStart = value; } }
    public Action OnUpdate { get { return _onLateUpdate; } set { _onLateUpdate = value; } }
    public Action OnLateUpdate { get { return _onUpdate; } set { _onUpdate = value; } }
    public Action OnCollidedWithPlayer { get { return _onCollidedWithPlayer; } set { _onCollidedWithPlayer = value; } }
    public Action<EnemyControl> OnCollidedWithEnemy { get { return _onCollidedWithEnemy;} set { _onCollidedWithEnemy = value; } }
    public Action OnCollidedWithOther { get { return _onCollidedWithOther; } set { _onCollidedWithOther = value; } }
    public Action OnDestroyed { get { return _onDestroyed; } set { _onDestroyed = value; } }
    public virtual void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        _supportObjControl = control;
    }
}
