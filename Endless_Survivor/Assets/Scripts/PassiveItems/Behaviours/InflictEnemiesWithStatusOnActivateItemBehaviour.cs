using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InflictEnemiesWithStatusOnActivateItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] List<RouletteElementChance<EnemyStatusEffectData>> _inflictedStatusEffects;
    [SerializeField] RandomBetweenTwoConstants _effectsPerEnemy;
    [SerializeField] ParticleSystem _particlesOnEnemyInflicted;
    [SerializeField] float _particlesDuration;
    [SerializeField] Vector2 _particlesOffset;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var inflictEffectsOriginal = original as InflictEnemiesWithStatusOnActivateItemBehaviour;
        _inflictedStatusEffects = new List<RouletteElementChance<EnemyStatusEffectData>>(inflictEffectsOriginal._inflictedStatusEffects);
        _effectsPerEnemy = inflictEffectsOriginal._effectsPerEnemy;
        _particlesOnEnemyInflicted = inflictEffectsOriginal._particlesOnEnemyInflicted;
        _particlesDuration = inflictEffectsOriginal._particlesDuration;
        _particlesOffset = inflictEffectsOriginal._particlesOffset;
    }
    public override void Activate()
    {
        base.Activate();
        var enemies = EnemySpawnManager.esm.Enemies;

        foreach (var enemy in enemies)
        {
            int effectsAmmount = (int)_effectsPerEnemy.rand;
            for(int i = 0; i < effectsAmmount; i++)
            {
                var addedEffectData = Utility.GetRouletteElement(_inflictedStatusEffects);
                enemy.GetComponent<EnemyControl>().StatusEffectManager.AddEffects(addedEffectData.StatusEffects, addedEffectData);

            }
            ParticleConfig particleConfig = new(_particlesOnEnemyInflicted, _particlesOffset, Quaternion.identity, _particlesDuration, enemy.transform, true, true);
            ParticleManager.pm.SpawnParticles(particleConfig);
        }
    }
}
