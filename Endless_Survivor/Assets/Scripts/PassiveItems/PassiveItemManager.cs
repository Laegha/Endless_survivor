using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveItemManager : MonoBehaviour
{
    List<PassiveItem> _passiveItems = new();

    private void Start()
    {
        PlayerControl.pc.PlayerHPManager.OnDamageTaken += PlayerDamaged;
        WaveManager.wm.OnEnemySpawned += (enemyControl) => enemyControl.EnemyHP.OnDeath += EnemyKilled;
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
    void EnemyKilled(EnemyControl killedEnemy)
    {
        _passiveItems.ForEach(item => item.BehaviourManager.onEnemyKilled?.Invoke(killedEnemy));
    }

}
