using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflictEnemiesWithStatsusOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<RouletteElementChance<EnemyStatusEffectData>> _inflictedStatusEffects;
    [SerializeField] RandomBetweenTwoConstants _effectsPerEnemy;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var inflictEffectsOriginal = original as InflictEnemiesWithStatsusOnActivateItemBehaviour;
        _inflictedStatusEffects = new List<RouletteElementChance<EnemyStatusEffectData>>(inflictEffectsOriginal._inflictedStatusEffects);
        _effectsPerEnemy = inflictEffectsOriginal._effectsPerEnemy;
    }
    public override void Activate()
    {
        base.Activate();
        var enemies = WaveManager.wm.Enemies;

        foreach (var enemy in enemies)
        {
            int effectsAmmount = (int)_effectsPerEnemy.rand;
            for(int i = 0; i < effectsAmmount; i++)
            {
                var addedEffectData = Utility.GetRouletteElement(_inflictedStatusEffects);
                enemy.GetComponent<EnemyControl>().StatusEffectManager.AddEffects(addedEffectData.StatusEffects, addedEffectData);

            }
        }
    }
}
