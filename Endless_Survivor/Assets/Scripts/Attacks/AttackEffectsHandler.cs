using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AttackEffectsHandler : MonoBehaviour
{
    List<AttackEffect> _activeEffects = new List<AttackEffect>();
    Attack _affectedAttack;

    public void TryEffects(Attack attack)
    {
        _affectedAttack = attack;
        var weaponEffects = attack.ParentWeapon != null ? attack.ParentWeapon.WeaponAttackEffects.availableEffects : new();
        var availableEffects = weaponEffects.Concat(PlayerControl.pc.EffectsHolder.availableEffects);
        foreach (AttackEffectData effectData in availableEffects)
        {
            if(!effectData.UsedBySecondaryAttacks && attack.IsSecondaryAttack)
                continue; 
            int rand = UnityEngine.Random.Range(0, 101);
            var activeEffects = effectData.GetActiveEffects(rand);
            foreach (var effect in activeEffects)
            {
                var effectInstance = Activator.CreateInstance(effect.GetType(), effect, attack);
                _activeEffects.Add((AttackEffect)effectInstance);
            }
        }
        foreach (AttackEffect effect in _activeEffects)
        {
            effect.OnAttack?.Invoke();
        }
    }
    void Update()
    {
        _activeEffects.ForEach(effect => effect.OnUpdate?.Invoke());
    }
    public void EnemyHit(EnemyControl enemyControl)
    {
        _activeEffects.ForEach(effect => effect.OnEnemyHit?.Invoke(enemyControl));
        if (_affectedAttack.TriggersPassiveItemHit)
            PlayerControl.pc.PassiveItemManager.EnemyHit(enemyControl, _affectedAttack);//THIS DEFINITELY SHOULDN'T BE HERE, BUT I DON'T KNOW HOW TO SOLVE THIS OTHERWISE :v


    }
}
