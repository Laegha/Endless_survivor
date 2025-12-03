using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackEffectsHolder
{
    public List<AttackEffectData> availableEffects = new();

    public void AddEffect(AttackEffectData effect) => availableEffects.Add(effect);
    public void RemoveEffect(AttackEffectData effectData) => availableEffects.Remove(effectData);
}
