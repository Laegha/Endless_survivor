using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectsHandler : MonoBehaviour
{
    List<AttackEffect> _activeEffects = new List<AttackEffect>();

    public void TryEffects(Attack attack)
    {
        var availableEffects = PlayerControl.pc.EffectsHolder.availableEffects;
        foreach (AttackEffectData effectData in availableEffects)
        {
            int rand = UnityEngine.Random.Range(0, 101);
            var activeEffects = effectData.GetActiveEffects(rand);
            foreach(var effect in activeEffects)
            {
                var effectInstance = Activator.CreateInstance(effect.GetType(), attack, effect);
                _activeEffects.Add((AttackEffect)effectInstance);
            }
        }
        foreach (AttackEffect effect in _activeEffects)
            effect.OnAttack();
    }
    void Update()
    {
        _activeEffects.ForEach(effect => effect.Update());
    }
    public void EnemyHit(EnemyControl enemyControl)
    {
        _activeEffects.ForEach(effect => effect.OnEnemyHit(enemyControl));

    }
}
