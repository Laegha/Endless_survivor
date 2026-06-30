using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PlayerGFXChanger
{
    //allow to stack
    //MAYBE add something related to costumes
    public MaterialOverride materialOverride;
    public ParticleSystem addedParticles;
    ParticleSystem _instantiatedParticles;
    bool _isApplied = false;

    public void ApplyGFX()
    {
        if (_isApplied)
            return;
        _isApplied = true;
        if(materialOverride != null)
            PlayerControl.pc.PlayerMaterialManager.SetMaterialOverride(materialOverride);
        if(addedParticles != null)
        {
            ParticleConfig addingParticles = new(addedParticles, PlayerControl.pc.transform.position, Quaternion.identity, -1, PlayerControl.pc.transform);
            _instantiatedParticles = ParticleManager.pm.SpawnParticles(addingParticles);

        }
    }
    public void ApplyGFX(float gfxDuration)
    {
        ApplyGFX();
        GameManager.gm.DelayAction(gfxDuration, UnApplyGFX, null);
    }
    public void UnApplyGFX()
    {
        _isApplied = false;
        PlayerControl.pc.PlayerMaterialManager.UnsetMaterialOverride(materialOverride);
        GameObject.Destroy(_instantiatedParticles);

    }
}
