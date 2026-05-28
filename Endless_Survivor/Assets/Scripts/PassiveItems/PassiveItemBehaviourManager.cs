using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemBehaviourManager
{
    PassiveItem _passiveItem;
    public Action onPicked;
    public Action onUpdate;
    public Action<WeaponAttackManager> onAttack;//make a system for attacks, so passive items can modify them
    public Action<EnemyControl, Attack> onEnemyHit;
    public Action<EnemyControl> onEnemyKilled;
    public Action<int> onPlayerDamaged;

    public PassiveItem PassiveItem { get { return _passiveItem; } }
    List<PassiveItemBehaviour> _itemBehaviours = new();
    public List<PassiveItemBehaviour> ItemBehaviours {  get { return _itemBehaviours; } }
    public PassiveItemBehaviourManager(PassiveItem passiveItem)
    {
        _passiveItem = passiveItem;
    }
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
