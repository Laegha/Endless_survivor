using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStatusEffectGroup : MonoBehaviour
{
    public PlayerStatusEffectData effectData;
    List<PlayerStatusEffect> _groupEffects;

    public PlayerStatusEffectGroup(PlayerStatusEffectData effectData, List<PlayerStatusEffect> groupEffects)
    {
        this.effectData = effectData;
        _groupEffects = groupEffects;
        foreach (PlayerStatusEffect effect in groupEffects)
            effect.ThisGroup = this;
    }
    public void Start()
    {
        foreach (PlayerStatusEffect effect in _groupEffects)
        {
            effect.Start();
        }
    }
    public void End()
    {
        foreach (PlayerStatusEffect effect in _groupEffects)
        {
            effect.End();
        }
    }
    public void Update()
    {
        foreach (PlayerStatusEffect effect in _groupEffects)
        {
            effect.Update();
        }
    }
    public void OnHit()
    {
        foreach (PlayerStatusEffect effect in _groupEffects)
            effect.PlayerHit();
    }
}
