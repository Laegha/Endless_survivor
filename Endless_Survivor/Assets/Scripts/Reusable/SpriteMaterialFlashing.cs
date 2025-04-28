using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteMaterialFlashing
{
    SpriteRenderer[] _affectedRenderers;
    float _flashTimer = 0;
    float _flashTime = .5f;
    Material _defaultMaterial;
    Material _flashingMaterial;
    Material _nextMaterial;

    public SpriteMaterialFlashing(SpriteRenderer[] affectedRenderers, float flashTime, Material defaultMaterial, Material flashingMaterial)
    {
        _affectedRenderers = affectedRenderers;
        _flashTime = flashTime;
        _defaultMaterial = defaultMaterial;
        _flashingMaterial = flashingMaterial;
    }

    public void Start()
    {
        _flashTimer = 0;
        ChangeMaterial(_flashingMaterial);
        _nextMaterial = _defaultMaterial;
    }

    public void Update()
    {
        _flashTimer += Time.deltaTime;
        if(_flashTimer > _flashTime)
        {
            _flashTimer = 0;
            ChangeMaterial(_nextMaterial);
            _nextMaterial = _nextMaterial == _defaultMaterial ? _flashingMaterial : _defaultMaterial;
        }
    }
    public void End()
    {
        ChangeMaterial(_defaultMaterial);
    }
    void ChangeMaterial(Material newMaterial)
    {
        foreach(var renderer in _affectedRenderers)
        {
            renderer.material = newMaterial;
        }
    }
}
