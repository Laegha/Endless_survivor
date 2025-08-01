using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackEffectsHandler : MonoBehaviour
{
    List<AttackEffect> _activeEffects = new List<AttackEffect>();

    public void TryEffects(Attack attack)
    {
        var availableEffects = attack.ParentWeapon.WeaponAttackEffects.availableEffects.Concat(PlayerControl.pc.EffectsHolder.availableEffects);
        foreach (AttackEffectData effectData in availableEffects)
        {
            int rand = UnityEngine.Random.Range(0, 101);
            var activeEffects = effectData.GetActiveEffects(rand);
            foreach(var effect in activeEffects)
            {
                var effectInstance = Activator.CreateInstance(effect.GetType(), effect, attack);
                print("ACTIVE EFFECT " + (AttackEffect) effectInstance);
                _activeEffects.Add((AttackEffect)effectInstance);
            }
        }
        foreach (AttackEffect effect in _activeEffects)
        {
            print("ACTIVATING " + effect);
            effect.OnAttack?.Invoke();
        }
    }
    void Update()
    {
        _activeEffects.ForEach(effect => effect.Update?.Invoke());
    }
    public void EnemyHit(EnemyControl enemyControl)
    {
        _activeEffects.ForEach(effect => effect.OnEnemyHit?.Invoke(enemyControl));

    }
}
