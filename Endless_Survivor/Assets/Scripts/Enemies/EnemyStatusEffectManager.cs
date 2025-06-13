using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;

public class EnemyStatusEffectManager : MonoBehaviour
{
    [SerializeField] EnemyControl _enemyControl;
    [SerializeField] SpriteGrid _statusIndicatorsGrid;
    readonly static int _materialAuthority =3;
    List<EnemyStatusEffect> _currentEffects = new();
    Dictionary<EnemyStatusEffect, EnemyStatusEffectGFX> _activeGfx = new();
    public void AddStatusGraphics(Sprite statusIcon, Material statusMaterial, EnemyStatusEffect status)
    {
        //display icon and add material to a kind of queue
        GridSpriteInfo statusSprite = null;
        if (statusIcon != null) 
            statusSprite = _statusIndicatorsGrid.AddSpriteToGrid(statusIcon);
        var materialOverride = new MaterialOverride(_materialAuthority, statusMaterial);
        if (statusMaterial != null)
        {
            _enemyControl.MaterialManager.SetMaterialOverride(materialOverride);

        }
        _activeGfx.Add(status, new EnemyStatusEffectGFX(statusSprite, materialOverride));
    }
    public void RemoveStatusGraphics(EnemyStatusEffect statusEffect)
    {
        _statusIndicatorsGrid.RemoveSpriteFromGrid(_activeGfx[statusEffect].statusSprite);
        _enemyControl.MaterialManager.UnsetMaterialOverride(_activeGfx[statusEffect].statusMaterial);
        _activeGfx.Remove(statusEffect);
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
    private void LateUpdate()
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
