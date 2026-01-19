using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WeaponInteraction
{
    public static int maxStacks => 0;
    WeaponInteractionData _interactionData;
    List<WeaponAttackManager> _affectedWeapons;

    public WeaponInteractionData InteractionData { get { return _interactionData; } }   

    public virtual void Initialize(WeaponInteraction original, WeaponInteractionData interactionData, List<WeaponAttackManager> affectedWeapons)
    {
        _affectedWeapons = new List<WeaponAttackManager>(affectedWeapons);
        _interactionData = interactionData;
    }
    public virtual void InteractionStart()
    {

    }
    public virtual void InteractionEnd()
    {

    }
}
