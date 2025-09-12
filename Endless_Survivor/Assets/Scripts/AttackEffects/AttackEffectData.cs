using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "New AttackEffect", menuName = "ScriptableObjects/AttackEffect", order = 1)]
public class AttackEffectData : ScriptableObject
{
    [SerializeField][Range(0, 100)] int _effectChance;
    [SerializeField] bool _usedBySecondaryAttacks = true;
    [SerializeReference] List<AttackEffect> _attackEffects = new List<AttackEffect>();

    public bool UsedBySecondaryAttacks { get { return _usedBySecondaryAttacks; } }
    public List<AttackEffect> AttackEffects { get { return _attackEffects; } }
    
    public List<AttackEffect> GetActiveEffects(int attackRand)
    {
        return _attackEffects.Where(x => attackRand <= (x.UsesSeparateChance ? x.EffectChance : _effectChance)).ToList();
    }
}
