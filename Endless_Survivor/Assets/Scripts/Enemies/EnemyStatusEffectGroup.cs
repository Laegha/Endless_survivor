using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffectGroup
{
    public EnemyStatusEffectData effectData;
    List<EnemyStatusEffect> _groupEffects;

    public EnemyStatusEffectGroup(EnemyStatusEffectData effectData, List<EnemyStatusEffect> groupEffects)
    {
        this.effectData = effectData;
        _groupEffects = groupEffects;
        foreach (EnemyStatusEffect effect in groupEffects)
            effect.ThisGroup = this;
    }
    public void Start()
    {
        foreach(EnemyStatusEffect effect in _groupEffects) 
        {
            effect.Start(); 
        }
    }
    public void End()
    {
        foreach (EnemyStatusEffect effect in _groupEffects)
        {
            effect.End();
        }
    }
    public void Update()
    {
        foreach (EnemyStatusEffect effect in _groupEffects)
        {
            effect.Update();
        }
    }
    public void OnKilled()
    {
        foreach (EnemyStatusEffect effect in _groupEffects)
            effect.EnemyKilled();
    }
    public void OnHit()
    {
        foreach (EnemyStatusEffect effect in _groupEffects)
            effect.EnemyHit();
    }
}
