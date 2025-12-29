using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "ScriptableObjects/Player Status Effect", fileName = "New PlayerStatusEffect", order = 6)]
public class PlayerStatusEffectData : ScriptableObject
{
    [SerializeReference] List<PlayerStatusEffect> _statusEffects = new();
    [SerializeField] int _effectMaxStacks;
    public List<PlayerStatusEffect> StatusEffects { get { return _statusEffects; } }
    public int EffectMaxStacks { get { return _effectMaxStacks; } }

    public void ApplyEffects(PlayerStatusEffectManager effectManager)
    {
        effectManager.AddEffects(_statusEffects, this);
    }
}
