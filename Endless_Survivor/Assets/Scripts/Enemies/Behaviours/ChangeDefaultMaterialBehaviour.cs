using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChangeDefaultMaterialBehaviour : EnemyBehaviour
{
    [SerializeField] bool _changeAllRenderers;
    [SerializeField] Material _defaultMat;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var changeMaterialOriginal = original as ChangeDefaultMaterialBehaviour;
        _changeAllRenderers = changeMaterialOriginal._changeAllRenderers;
        _defaultMat = changeMaterialOriginal._defaultMat;
    }
    public override void Start()
    {
        base.Start();
        MaterialOverride defaultMaterialOverride = new(-1, _defaultMat);
        EnemyControl.MaterialManager.SetMaterialOverride(defaultMaterialOverride);
    }
}