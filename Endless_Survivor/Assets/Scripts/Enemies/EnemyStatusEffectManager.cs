using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStatusEffectManager : MonoBehaviour
{
    [SerializeField] EnemyControl _enemyControl;
    [SerializeField] SpriteGrid _statusIndicatorsGrid;
    List<EnemyStatusEffect> _currentEffects = new();

    public void AddStatusGraphics(Sprite statusIcon, Material statusMaterial)
    {
        //display icon and add material to a kind of queue
        _statusIndicatorsGrid.AddSpriteToGrid(statusIcon);
    }
    public void RemoveStatusGraphics()
    {

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
