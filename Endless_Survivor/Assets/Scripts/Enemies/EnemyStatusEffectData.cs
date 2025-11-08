using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyStatusEffect", menuName = "ScriptableObjects/EnemyStatusEffect", order = 1)]
public class EnemyStatusEffectData : ScriptableObject
{
    [SerializeReference] List<EnemyStatusEffect>_statusEffects = new();
    public List<EnemyStatusEffect> StatusEffects { get {  return _statusEffects; } }

    public void ApplyEffects(EnemyStatusEffectManager effectManager)
    {
        foreach(var effect in _statusEffects)
        {
            effectManager.AddEffect(effect);
        }
    }
}
