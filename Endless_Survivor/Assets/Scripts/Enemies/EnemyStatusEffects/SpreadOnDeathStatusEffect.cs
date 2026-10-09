using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpreadOnDeathStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [Tooltip("-1 for any enemy in the map")][SerializeField] float _spreadMaxDist;
    [Range(0, 100)] [SerializeField] float _spreadChance;

    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        var spreadOnDeathOriginal = original as SpreadOnDeathStatusEffect;
        _spreadMaxDist = spreadOnDeathOriginal._spreadMaxDist;
        _spreadChance = spreadOnDeathOriginal._spreadChance;
    }
    public override void EnemyKilled()
    {
        base.EnemyKilled();
        float rand = Random.Range(0, 100);
        if (rand > _spreadChance)
            return;
        List<GameObject> enemies = new(EnemySpawnManager.esm.Enemies);
        if(_spreadMaxDist > 0) 
            enemies.RemoveAll(enemy => Vector2.Distance(enemy.transform.position, AffectedEnemyControl.transform.position) > _spreadMaxDist);
        if (enemies.Count == 0)
            return;
        EnemyControl affectedEnemy = enemies[Random.Range(0, enemies.Count)].GetComponent<EnemyControl>();
        ThisGroup.effectData.ApplyEffects(affectedEnemy.StatusEffectManager);
    }
}
