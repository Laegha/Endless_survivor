using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class EnemyBehaviour
{
    EnemyData _enemyData;
    [SerializeReference] List<EnemyBehaviour> _overrideBehaviours = new List<EnemyBehaviour>();
    EnemyControl _enemyControl;

    public EnemyData EnemyData { get { return _enemyData; } set { _enemyData = value; } }
    public List<EnemyBehaviour> OverrideBehaviours { get { return _overrideBehaviours; } }

    public EnemyBehaviour(EnemyBehaviour original)
    {
        _enemyData = original.EnemyData;
        _overrideBehaviours = original.OverrideBehaviours;
    }

    public List<EnemyBehaviour> EnemyDataBehaviours()
    {
        return _enemyData.EnemyBehaviours;
    }

    public virtual void Start(EnemyControl enemyControl)
    {
        _enemyControl = enemyControl;
        _overrideBehaviours = _overrideBehaviours.Where(behaviour => behaviour != null).ToList();
    }

    public virtual void PassiveUpdate() { }//is called every frame, regardless the state of the behaviour

    public virtual void ActiveUpdate() { }//is called every frame only when the behaviour is executing
}
