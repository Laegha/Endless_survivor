using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New EnemyStatusEffect", menuName = "ScriptableObjects/EnemyStatusEffect", order = 1)]
public class EnemyStatusEffectData : ScriptableObject
{
    [SerializeReference] List<EnemyStatusEffect>_statusEffects = new();
    [SerializeField] int _effectMaxStacks;
    [SerializeField] float _effectApplyCooldown;
    public List<EnemyStatusEffect> StatusEffects { get {  return _statusEffects; } }
    public int EffectMaxStacks { get { return _effectMaxStacks; } }
    public float EffectApplyCooldown { get { return _effectApplyCooldown; } }

    public void ApplyEffects(EnemyStatusEffectManager effectManager)
    {
        effectManager.AddEffects(_statusEffects, this);
    }
}
