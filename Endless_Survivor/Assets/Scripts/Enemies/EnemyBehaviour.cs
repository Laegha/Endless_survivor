using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class EnemyBehaviour
{
    public static int maxStacks => 0;
    bool _isActive = false;
    EnemyData _enemyData;
    [SerializeField] string _behaviourId;
    [SerializeReference] List<string> _overrideBehaviours = new List<string>();
    EnemyControl _enemyControl;

    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public string BehaviourId { get { return _behaviourId; } }
    public EnemyData EnemyData { get { return _enemyData; } set { _enemyData = value; } }
    public List<string> OverrideBehaviours { get { return _overrideBehaviours; } }
    public EnemyControl EnemyControl { get {  return _enemyControl; } } 

    public List<EnemyBehaviour> EnemyDataBehaviours()
    {
        return _enemyData.EnemyBehaviours;
    }

    public virtual void Initialize(EnemyBehaviour original, EnemyControl enemyControl)//called when the behaviour is added to the list
    {
        _enemyData = original.EnemyData;
        _overrideBehaviours = original.OverrideBehaviours;
        _enemyControl = enemyControl;
        _behaviourId = original.BehaviourId;
    }

    public virtual void Start() { } //called after every behaviour has been initialized

    public void RewriteOverrides(List<EnemyBehaviour> totalBehaviours)
    {
        _overrideBehaviours = _overrideBehaviours.Where(behaviour => _enemyControl.BehaviourManager.GetBehaviour(behaviour) != null).ToList();
    }

    public virtual void PassiveUpdate() { }//is called every frame, regardless the state of the behaviour

    public virtual void ActiveUpdate() { }//is called every frame only when the behaviour is executing

    public virtual void KillBehaviour()//BEWARE: there is a frame of delay between the kill behaviour and the last ActiveUpdate, to be considered while developing precise behaviour on ActiveUpdate
    {
        _enemyControl.BehaviourManager.KillBehaviour(this);
    }
}
