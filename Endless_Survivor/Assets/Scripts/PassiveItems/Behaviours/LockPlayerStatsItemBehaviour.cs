using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LockPlayerStatsItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] PlayerStatLockInfo _maxHpLock;
    [SerializeField] PlayerStatLockInfo _hpRegenLock;
    [SerializeField] PlayerStatLockInfo _maxSpeedLock;


    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var lockStatsOriginal = original as LockPlayerStatsItemBehaviour;
        _maxHpLock = lockStatsOriginal._maxHpLock;
        _hpRegenLock = lockStatsOriginal._hpRegenLock;
        _maxSpeedLock = lockStatsOriginal._maxSpeedLock;

    }

    void KeepPlayerStats()
    {
        if(_maxHpLock.isLocked)
        {
            float min = _maxHpLock.minInfinity ? Mathf.Infinity : _maxHpLock.min;
            float max = _maxHpLock.maxInfinity ? Mathf.Infinity : _maxHpLock.max;
            Mathf.Clamp(PlayerControl.pc.PlayerHPManager.MaxHP, min, max);
        }
        if (_hpRegenLock.isLocked)
        {
            float min = _hpRegenLock.minInfinity ? Mathf.Infinity : _hpRegenLock.min;
            float max = _hpRegenLock.maxInfinity ? Mathf.Infinity : _hpRegenLock.max;
            Mathf.Clamp(PlayerControl.pc.PlayerStats.HPRegeneration, min, max);
        }
        if (_maxSpeedLock.isLocked)
        {
            float min = _maxSpeedLock.minInfinity ? Mathf.Infinity : _maxSpeedLock.min;
            float max = _maxSpeedLock.maxInfinity ? Mathf.Infinity : _maxSpeedLock.max;
            Mathf.Clamp(PlayerControl.pc.PlayerStats.MaxSpeed, min, max);
        }
    }
    public override void RemoveBehaviour()
    {

    }
}

[Serializable]
class PlayerStatLockInfo
{
    public bool isLocked;
    public float min;
    public bool minInfinity;
    public float max;
    public bool maxInfinity;
}