using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemBehaviourManager
{
    public Action onPicked;
    public Action onUpdate;
    public Action<WeaponAttackManager> onAttack;//make a system for attacks, so passive items can modify them
    public Action onDamageRecieved;
    public Action<EnemyControl, Attack> onEnemyHit;
    public Action<EnemyControl> onEnemyKilled;
    public Action onPlayerDamaged;

    List<PassiveItemBehaviour> _itemBehaviours = new();
    public List<PassiveItemBehaviour> ItemBehaviours {  get { return _itemBehaviours; } }
    public void CopyBehaviours(List<PassiveItemBehaviour> itemBehaviours)
    {
        foreach (PassiveItemBehaviour behaviour in itemBehaviours)
        {
            PassiveItemBehaviour addedBehaviour = (PassiveItemBehaviour)Activator.CreateInstance(behaviour.GetType());
            addedBehaviour.CopyValues(behaviour, this);
            _itemBehaviours.Add(addedBehaviour);
        }
    }
    public void RemoveBehaviours()
    {
        foreach(var behaviour in _itemBehaviours)
        {
            behaviour.RemoveBehaviour();
        }
    }
}
