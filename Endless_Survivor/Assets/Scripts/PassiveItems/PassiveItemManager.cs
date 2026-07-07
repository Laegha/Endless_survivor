using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    List<PassiveItem> _passiveItems = new();
    Action _onItemPickup;
    Action _onItemRemoved;
    List<PassiveItem> _overridenPassiveItems = new();
    public List<PassiveItem> PassiveItems { get { return _passiveItems; } }
    public Action OnItemPickup {  get { return _onItemPickup; } set { _onItemPickup = value; } }
    public Action OnItemRemoved { get { return _onItemRemoved; } set { _onItemRemoved = value; } }

    private void Start()
    {
        PlayerControl.pc.PlayerHPManager.OnDamageTaken += PlayerDamaged;
        EnemySpawnManager.esm.OnEnemySpawned += (enemyControl) => enemyControl.EnemyHP.OnDeath += EnemyKilled;
        IntensityManager.im.OnLevelIncrease += LevelIncrease;
    }
    public PassiveItem AddPassiveItem(PassiveItemData itemData)
    {
        PassiveItem addedItem = new PassiveItem();
        itemData.TransferData(addedItem);
        addedItem.BehaviourManager.onPicked?.Invoke();

        _passiveItems.Add(addedItem);

        foreach (var itemOverride in itemData.ItemOverrides)
        {
            PassiveItem overridenItem = _passiveItems.Find(x => x.ItemData == itemOverride.OverridenItem);
            if (overridenItem == null)
                continue;
            if (itemOverride.IsItemRemovedPerm)
            {
                RemovePassiveItem(overridenItem);
                continue;
            }
            if (_overridenPassiveItems.Contains(overridenItem))
                continue;
            overridenItem.RemoveItem();
            _overridenPassiveItems.Add(overridenItem);
        }


        //if (!_passiveItems.Any(item => item.ItemData.ItemOverrides.Any(over => over.OverridenItem == itemData)))
        //{
            
        //}

        //else
        //{
            //_overridenPassiveItems.Add(addedItem);
            //addedItem.RemoveItem();
        //}

        return addedItem;
    }
    public void RemovePassiveItem(PassiveItem removedItem)
    {
        _passiveItems.Remove(removedItem);
        foreach(var itemOverride in removedItem.ItemData.ItemOverrides)
        {
            PassiveItem overridingItem = _overridenPassiveItems.Find(x => x.ItemData == itemOverride.OverridenItem);
            if(overridingItem == null)
                continue;
            //checking if it is on the list is a failsafe in case the new item and the existing one override each other
            if (_overridenPassiveItems.Contains(overridingItem) && _passiveItems.Any(item => item.ItemData.ItemOverrides.Any(over => over.OverridenItem == overridingItem.ItemData)))
                continue;
            _overridenPassiveItems.Remove(overridingItem);
            overridingItem.BehaviourManager.onPicked?.Invoke();
        }

        removedItem.RemoveItem();
    }
    private void Update()
    {
        var passiveItemsCopy = new List<PassiveItem>(_passiveItems);
        foreach (var item in passiveItemsCopy)
        {
            if(_overridenPassiveItems.Contains(item))
                continue;
            item.BehaviourManager.onUpdate?.Invoke();
        }

    }
    public void WeaponAttack(WeaponAttackManager weapon)
    {
        var passiveItemsCopy = new List<PassiveItem>(_passiveItems);
        foreach (var item in passiveItemsCopy)
        {
            if (_overridenPassiveItems.Contains(item))
                continue;
            item.BehaviourManager.onAttack?.Invoke(weapon);
        }

    }
    void PlayerDamaged(int damage)
    {
        var passiveItemsCopy = new List<PassiveItem>(_passiveItems);
        foreach (var item in passiveItemsCopy)
        {
            if (_overridenPassiveItems.Contains(item))
                continue;
            item.BehaviourManager.onPlayerDamaged?.Invoke(damage);
        }

    }
    public void EnemyHit(EnemyControl hitEnemy, Attack hitAttack)
    {
        var passiveItemsCopy = new List<PassiveItem>(_passiveItems);
        foreach (var item in passiveItemsCopy)
        {
            if (_overridenPassiveItems.Contains(item))
                continue;
            item.BehaviourManager.onEnemyHit?.Invoke(hitEnemy, hitAttack);
        }
    }
    void EnemyKilled(EnemyControl killedEnemy)
    {
        var passiveItemsCopy = new List<PassiveItem>(_passiveItems);
        foreach (var item in passiveItemsCopy)
        {
            if (_overridenPassiveItems.Contains(item))
                continue;
            item.BehaviourManager.onEnemyKilled?.Invoke(killedEnemy);
        }
    }

    void LevelIncrease()
    {
        var passiveItemsCopy = new List<PassiveItem>(_passiveItems);
        foreach (var item in passiveItemsCopy)
        {
            if (_overridenPassiveItems.Contains(item))
                continue;
            item.BehaviourManager.onIntensityIncrease?.Invoke();
        }
    }

}
