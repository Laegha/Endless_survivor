using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerStatusEffectManager : MonoBehaviour
{
    [SerializeField] SpriteGrid _statusIndicatorsGrid;
    readonly static int _materialAuthority = 3;
    List<PlayerStatusEffectGroup> _currentEffects = new();
    Dictionary<PlayerStatusEffect, StatusEffectGFX> _activeGfx = new();

    public void SetGridLocalPos(Vector2 pos) => _statusIndicatorsGrid.transform.localPosition = pos;

    public void AddStatusGraphics(Sprite statusIcon, Material statusMaterial, ParticleSystem statusParticles, Vector2 particlesOffset, PlayerStatusEffect status)
    {
        //display icon and add material to a kind of queue
        GridSpriteInfo statusSprite = null;
        if (statusIcon != null)
            statusSprite = _statusIndicatorsGrid.AddSpriteToGrid(statusIcon);
        var materialOverride = new MaterialOverride(_materialAuthority, statusMaterial);
        if (statusMaterial != null)
        {
            PlayerControl.pc.PlayerMaterialManager.SetMaterialOverride(materialOverride);

        }
        ParticleSystem instantiatedParticles = null;
        if (statusParticles != null)
        {
            ParticleConfig statusParticeConfig = new(statusParticles, particlesOffset, transform.rotation, -1, transform, true, true);
            instantiatedParticles = ParticleManager.pm.SpawnParticles(statusParticeConfig);

        }
        _activeGfx.Add(status, new StatusEffectGFX(statusSprite, materialOverride, instantiatedParticles));
    }
    public void RemoveStatusGraphics(PlayerStatusEffect statusEffect)
    {
        _statusIndicatorsGrid.RemoveSpriteFromGrid(_activeGfx[statusEffect].statusSprite);
        PlayerControl.pc.PlayerMaterialManager.UnsetMaterialOverride(_activeGfx[statusEffect].statusMaterial);
        if (_activeGfx[statusEffect].statusParticles != null)
            Destroy(_activeGfx[statusEffect].statusParticles);
        _activeGfx.Remove(statusEffect);
    }
    public void AddEffects(List<PlayerStatusEffect> originalEffects, PlayerStatusEffectData effectData, bool byPassesInmunity = false)
    {
        if (!byPassesInmunity && PlayerControl.pc.PlayerHPManager.IsInmune)
            return;
        var currEffectStacks = _currentEffects.Where(x => x.effectData == effectData).Count();
        if (currEffectStacks >= effectData.EffectMaxStacks)
            return;

        List<PlayerStatusEffect> newEffects = new();
        foreach (PlayerStatusEffect originalEffect in originalEffects)
        {
            PlayerStatusEffect newEffect = (PlayerStatusEffect)Activator.CreateInstance(originalEffect.GetType());
            newEffect.Initialize(originalEffect);
            newEffects.Add(newEffect);
        }
        PlayerStatusEffectGroup newEffectGroup = new(effectData, newEffects);
        newEffectGroup.Start();
        _currentEffects.Add(newEffectGroup);
    }
    public void RemoveEffect(PlayerStatusEffect effect)
    {
        var effectGroup = _currentEffects.Find(x => x == effect.ThisGroup);
        effectGroup.End();
        _currentEffects.Remove(effectGroup);
    }
    private void Update()
    {
        var activeEffects = new List<PlayerStatusEffectGroup>(_currentEffects);
        foreach (PlayerStatusEffectGroup effect in activeEffects)
        {
            effect.Update();
        }
    }
    public void OnHit()
    {
        foreach (PlayerStatusEffectGroup effect in _currentEffects)
            effect.OnHit();
    }
}
