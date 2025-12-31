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
    List<EnemyStatusEffectGroup> _currentEffects = new();
    Dictionary<EnemyStatusEffect, StatusEffectGFX> _activeGfx = new();

    public void SetGridLocalPos(Vector2 pos) => _statusIndicatorsGrid.transform.localPosition = pos;    

    public void AddStatusGraphics(Sprite statusIcon, Material statusMaterial, ParticleSystem statusParticles, Vector2 particlesOffset, EnemyStatusEffect status)
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
        ParticleSystem instantiatedParticles = null;
        if(statusParticles != null)
        {
            ParticleConfig statusParticeConfig = new(statusParticles, Vector2.zero, transform.rotation, -1, transform, true, true);
            instantiatedParticles = ParticleManager.pm.SpawnParticles(statusParticeConfig);

        }
        _activeGfx.Add(status, new StatusEffectGFX(statusSprite, materialOverride, instantiatedParticles));
    }
    public void RemoveStatusGraphics(EnemyStatusEffect statusEffect)
    {
        _statusIndicatorsGrid.RemoveSpriteFromGrid(_activeGfx[statusEffect].statusSprite);
        _enemyControl.MaterialManager.UnsetMaterialOverride(_activeGfx[statusEffect].statusMaterial);
        if(_activeGfx[statusEffect].statusParticles!= null)
            Destroy(_activeGfx[statusEffect].statusParticles);
        _activeGfx.Remove(statusEffect);
    }
    public void AddEffects(List<EnemyStatusEffect> originalEffects, EnemyStatusEffectData effectData)
    {
        var currEffectStacks = _currentEffects.Where(x => x.effectData == effectData).Count();
        if (currEffectStacks >= effectData.EffectMaxStacks)
            return;

        List<EnemyStatusEffect> newEffects = new();
        foreach (EnemyStatusEffect originalEffect in originalEffects)
        {
            EnemyStatusEffect newEffect = (EnemyStatusEffect)Activator.CreateInstance(originalEffect.GetType());
            newEffect.Initialize(_enemyControl, originalEffect);
            newEffects.Add(newEffect);
        }
        EnemyStatusEffectGroup newEffectGroup = new(effectData, newEffects);
        newEffectGroup.Start();
        _currentEffects.Add(newEffectGroup);
    }
    public void RemoveEffect(EnemyStatusEffect effect)
    {
        var effectGroup = _currentEffects.Find(x => x == effect.ThisGroup);
        effectGroup.End();
        _currentEffects.Remove(effectGroup);
    }
    private void Update()
    {
        var activeEffects = new List<EnemyStatusEffectGroup>(_currentEffects);
        foreach (EnemyStatusEffectGroup effect in activeEffects) 
        {
            effect.Update();
        }
    }
    public void OnKilled()
    {
        foreach (EnemyStatusEffectGroup effect in _currentEffects) 
            effect.OnKilled();
    }
    public void OnHit()
    {
        foreach(EnemyStatusEffectGroup effect in _currentEffects) 
            effect.OnHit();
    }
}
