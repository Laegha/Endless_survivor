using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyStatusEffectManager : MonoBehaviour
{
    [SerializeField] EnemyControl _enemyControl;
    [SerializeField] SpriteGrid _statusIndicatorsGrid;
    List<EnemyStatusEffect> _currentEffects = new();
    Dictionary<EnemyStatusEffect, EnemyStatusEffectGFX> _activeGfx;

    public void AddStatusGraphics(Sprite statusIcon, Material statusMaterial, EnemyStatusEffect status)
    {
        //display icon and add material to a kind of queue
        var statusSprite = _statusIndicatorsGrid.AddSpriteToGrid(statusIcon);
        _activeGfx.Add(status, new EnemyStatusEffectGFX(statusSprite, statusMaterial));
    }
    public void RemoveStatusGraphics(EnemyStatusEffect statusEffect)
    {
        _statusIndicatorsGrid.RemoveSpriteFromGrid(_activeGfx[statusEffect].statusSprite);
        _activeGfx.Remove(statusEffect);
        Material updatedMaterial = _activeGfx.Count == 0 ? _activeGfx.Last().Value.statusMaterial /*<-change this to the default material*/ : _activeGfx.Last().Value.statusMaterial;
        
    }
    public void AddEffect(EnemyStatusEffect effect)
    {
        EnemyStatusEffect newEffect = (EnemyStatusEffect)Activator.CreateInstance(effect.GetType());
        newEffect.Initialize(_enemyControl, effect);
        newEffect.Start();
        _currentEffects.Add(newEffect);
    }
    public void RemoveEffect(EnemyStatusEffect effect)
    {
        effect.End();
        _currentEffects.Remove(effect);
    }
    private void Update()
    {
        foreach (EnemyStatusEffect effect in _currentEffects) 
        {
            effect.Update(); 
        }
    }
    public void OnKilled()
    {
        foreach (EnemyStatusEffect effect in _currentEffects) 
            effect.EnemyKilled();
    }
    public void OnHit()
    {
        foreach(EnemyStatusEffect effect in _currentEffects) 
            effect.EnemyHit();
    }
}
