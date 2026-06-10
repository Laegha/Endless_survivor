using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    List<PassiveItem> _passiveItems = new();
    Action _onItemPickup;
    Action _onItemRemoved;
    public List<PassiveItem> PassiveItems { get { return _passiveItems; } }
    public Action OnItemPickup {  get { return _onItemPickup; } set { _onItemPickup = value; } }
    public Action OnItemRemoved { get { return _onItemRemoved; } set { _onItemRemoved = value; } }

    private void Start()
    {
        PlayerControl.pc.PlayerHPManager.OnDamageTaken += PlayerDamaged;
        EnemySpawnManager.esm.OnEnemySpawned += (enemyControl) => enemyControl.EnemyHP.OnDeath += EnemyKilled;
    }
    public PassiveItem AddPassiveItem(PassiveItemData itemData)
    {
        PassiveItem addedItem = new PassiveItem();
        itemData.TransferData(addedItem);
        addedItem.BehaviourManager.onPicked?.Invoke();
        _passiveItems.Add(addedItem);
        return addedItem;
    }
    public void RemovePassiveItem(PassiveItem removedItem)
    {
        _passiveItems.Remove(removedItem);
        removedItem.RemoveItem();
    }
    private void Update()
    {
        var passiveItemsCopy = new List<PassiveItem>(_passiveItems);
        passiveItemsCopy.ForEach(item => item.BehaviourManager.onUpdate?.Invoke());
    }
    public void WeaponAttack(WeaponAttackManager weapon)
    {
        var passiveItemsCopy = new List<PassiveItem>(_passiveItems);
        passiveItemsCopy.ForEach(item => item.BehaviourManager.onAttack?.Invoke(weapon));

    }
    void PlayerDamaged(int damage)
    {
        var passiveItemsCopy = new List<PassiveItem>(_passiveItems);
        passiveItemsCopy.ForEach(item => item.BehaviourManager.onPlayerDamaged?.Invoke(damage));

    }
    public void EnemyHit(EnemyControl hitEnemy, Attack hitAttack)
    {
        var passiveItemsCopy = new List<PassiveItem>(_passiveItems);
        passiveItemsCopy.ForEach(item => item.BehaviourManager.onEnemyHit?.Invoke(hitEnemy, hitAttack));
    }
    void EnemyKilled(EnemyControl killedEnemy)
    {
        var passiveItemsCopy = new List<PassiveItem>(_passiveItems);
        passiveItemsCopy.ForEach(item => item.BehaviourManager.onEnemyKilled?.Invoke(killedEnemy));
    }

}
