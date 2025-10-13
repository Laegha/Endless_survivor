using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ChangeDefaultMaterialBehaviour : EnemyBehaviour
{
    [SerializeField] bool _changeAllRenderers;
    [Header("USE ONLY IF CHANGEALLRENDERERS IS FALSE")]
    [SerializeField] List<RendererPathMaterialInfo> _separateRenderersMaterials;
    [Header("USE ONLY IF CHANGEALLRENDERERS IS TRUE")]
    [SerializeField] Material _globalMaterial;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var changeMaterialOriginal = original as ChangeDefaultMaterialBehaviour;
        _changeAllRenderers = changeMaterialOriginal._changeAllRenderers;
        _separateRenderersMaterials = changeMaterialOriginal._separateRenderersMaterials;
        _globalMaterial = changeMaterialOriginal._globalMaterial;
    }
    public override void Start()
    {
        base.Start();
        if(_changeAllRenderers)
            EnemyControl.MaterialManager.ChangeDefaultMaterial(_globalMaterial);
        else
        {
            foreach(var renderer in _separateRenderersMaterials)
            {
                EnemyControl.MaterialManager.ChangeDefaultMaterial(renderer.rendererPath, renderer.material);
            }
        }
        EnemyControl.gameObject.name = "TESTING MATERIAL CHANFGE";
    }
}