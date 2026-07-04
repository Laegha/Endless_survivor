using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PokerHandPattern = PokerHand.PokerHandPattern;

[Serializable]
public class PokerHandEffectsInfo
{
    [SerializeField] PokerHandPattern _triggeringPattern;
    [SerializeField] List<AttackEffectData> _triggeredEffects;
    [SerializeField] ParticleSystem _patternParticles;
    public PokerHandPattern TriggeringPattern { get { return _triggeringPattern; } }
    public List<AttackEffectData> TriggeredEffects { get { return _triggeredEffects; } }
    public ParticleSystem PatternParticles { get { return _patternParticles; } }
}
