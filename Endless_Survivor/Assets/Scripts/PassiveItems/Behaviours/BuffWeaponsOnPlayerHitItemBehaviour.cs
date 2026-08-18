using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class BuffWeaponsOnPlayerHitItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [Tooltip("Here, wait for external means it will be debuffed when this item is removed")][SerializeField] WeaponBuffData _buffData;
    [SerializeField] float _chanceOfHappenning = 100f;
    //ADD GFX CHANGE TO THE PLAYER!!!!
    [SerializeField] PlayerGFXChanger _gfxChanger;
    [SerializeField] SFXInfo _onBuffSFX;

    int _givenStacks = 0;

    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var buffWeaponsOriginal = original as BuffWeaponsOnPlayerHitItemBehaviour;
        _buffData = buffWeaponsOriginal._buffData;
        _chanceOfHappenning = buffWeaponsOriginal._chanceOfHappenning;
        _gfxChanger = new(buffWeaponsOriginal._gfxChanger);
        _onBuffSFX = buffWeaponsOriginal._onBuffSFX;

        behaviourManager.onPlayerDamaged += TryBuffStats;
    }
    void TryBuffStats(int _)
    {
        float rand = Random.Range(0, 100f);
        if (rand > _chanceOfHappenning)
            return;

        if (!WeaponBuffsManager.wbm.AddBuffStack(_buffData))
            return;

        _givenStacks++;
        _gfxChanger.ApplyGFX();
        SoundFXManager.sm.PlaySfx(_onBuffSFX, PlayerControl.pc.transform.position);
    }
    //void DecreaseStacks(WeaponBuffHandler removedBuffHandler)
    //{
        //_activeBuffHandlers.Remove(removedBuffHandler);
        //if (_activeBuffHandlers.Count == 0 || _activeBuffHandlers[0].BuffCurrentStacks == 0)
            //_gfxChanger.UnApplyGFX();
    //}

    public override void RemoveBehaviour()
    {
        if (_buffData.DurationType != WeaponBuffHandler.BuffDurationType.WaitForExternal)
            return;

        for (int i = 0; i < _givenStacks; i++)
        {
            WeaponBuffsManager.wbm.RemoveBuffStack(_buffData);
        }
    }
}