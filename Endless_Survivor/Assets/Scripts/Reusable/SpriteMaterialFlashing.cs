using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SpriteMaterialFlashing
{
    MaterialManager _materialManager;
    float _flashTimer = 0;
    float _flashTime = .5f;
    MaterialOverride _flashingMaterialOverride;
    bool _isActive = false;

    public SpriteMaterialFlashing(MaterialManager materialManager, float flashTime, MaterialOverride materialOverride)
    {
        _materialManager = materialManager;
        _flashTime = flashTime;
        _flashingMaterialOverride = materialOverride;
    }

    public void Start()
    {
        _flashTimer = 0;
        ToggleMaterial();
    }

    public void Update()
    {
        _flashTimer += Time.deltaTime;
        if(_flashTimer > _flashTime)
        {
            _flashTimer = 0;
            ToggleMaterial();
        }
    }
    public void End()
    {
        _materialManager.UnsetMaterialOverride(_flashingMaterialOverride);
    }
    void ToggleMaterial()
    {
        _isActive = !_isActive;
        if( _isActive )
            _materialManager.SetMaterialOverride(_flashingMaterialOverride);
        else
            _materialManager.UnsetMaterialOverride(_flashingMaterialOverride);
    }

}
