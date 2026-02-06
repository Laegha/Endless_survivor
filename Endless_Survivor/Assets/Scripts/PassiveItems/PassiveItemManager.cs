using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    List<PassiveItem> _passiveItems = new();

    private void Start()
    {
        PlayerControl.pc.PlayerHPManager.OnDamageTaken += PlayerDamaged;
        EnemySpawnManager.esm.OnEnemySpawned += (enemyControl) => enemyControl.EnemyHP.OnDeath += EnemyKilled;
    }
    public void AddPassiveItem(PassiveItemData itemData)
    {
        PassiveItem addedItem = new PassiveItem();
        itemData.TransferData(addedItem);
        addedItem.BehaviourManager.onPicked?.Invoke();
        _passiveItems.Add(addedItem);
    }
    private void Update()
    {
        _passiveItems.ForEach(item => item.BehaviourManager.onUpdate?.Invoke());
    }
    public void WeaponAttack(WeaponAttackManager weapon)
    {
        _passiveItems.ForEach(item => item.BehaviourManager.onAttack?.Invoke(weapon));

    }
    void PlayerDamaged()
    {
        _passiveItems.ForEach(item => item.BehaviourManager.onPlayerDamaged?.Invoke());

    }
    public void EnemyHit(EnemyControl hitEnemy, Attack hitAttack)
    {
        _passiveItems.ForEach(item => item.BehaviourManager.onEnemyHit?.Invoke(hitEnemy, hitAttack));
    }
    void EnemyKilled(EnemyControl killedEnemy)
    {
        _passiveItems.ForEach(item => item.BehaviourManager.onEnemyKilled?.Invoke(killedEnemy));
    }

}
