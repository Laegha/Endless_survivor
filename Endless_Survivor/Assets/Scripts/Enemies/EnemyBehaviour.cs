using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class EnemyBehaviour
{
    bool _isActive = false;
    EnemyData _enemyData;
    [SerializeReference] List<EnemyBehaviour> _overrideBehaviours = new List<EnemyBehaviour>();
    EnemyControl _enemyControl;

    public bool IsActive { get { return _isActive; } set { _isActive = value; } }
    public EnemyData EnemyData { get { return _enemyData; } set { _enemyData = value; } }
    public List<EnemyBehaviour> OverrideBehaviours { get { return _overrideBehaviours; } }
    public EnemyControl EnemyControl { get {  return _enemyControl; } } 

    public List<EnemyBehaviour> EnemyDataBehaviours()
    {
        return _enemyData.EnemyBehaviours;
    }

    public virtual void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        _enemyData = original.EnemyData;
        _overrideBehaviours = original.OverrideBehaviours;
        _enemyControl = enemyControl;
        _overrideBehaviours = _overrideBehaviours.Where(behaviour => behaviour != null).ToList();
    }

    public void RewriteOverrides(List<EnemyBehaviour> totalBehaviours)
    {
        List<EnemyBehaviour> rewrittenBehaviours = new List<EnemyBehaviour>();
        foreach(EnemyBehaviour overrideBehaviour in _overrideBehaviours)
        {
            rewrittenBehaviours.Add(totalBehaviours.Where(behaviour => behaviour.GetType() == overrideBehaviour.GetType()).ToList()[0]);
        }
        _overrideBehaviours = rewrittenBehaviours;
    }

    public virtual void PassiveUpdate() { }//is called every frame, regardless the state of the behaviour

    public virtual void ActiveUpdate() { }//is called every frame only when the behaviour is executing

    public virtual void KillBehaviour()//BEWARE: there is a tick of delay between the kill behaviour and the last ActiveUpdate, to be considered while developing precise behaviour on ActiveUpdate
    {
        _enemyControl.BehaviourManager.KillBehaviour(this);
    }
}
