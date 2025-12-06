using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LockPlayerStatsItemBehaviour : PassiveItemBehaviour
{
    new public static bool isUsable => true;
    [SerializeField] PlayerStatLockInfo _maxHpLock;
    [SerializeField] PlayerStatLockInfo _hpRegenLock;
    [SerializeField] PlayerStatLockInfo _speedLock;


    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var lockStatsOriginal = original as LockPlayerStatsItemBehaviour;
        _maxHpLock = lockStatsOriginal._maxHpLock;
        _hpRegenLock = lockStatsOriginal._hpRegenLock;
        _speedLock = lockStatsOriginal._speedLock;

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
        if (_speedLock.isLocked)
        {
            float min = _speedLock.minInfinity ? Mathf.Infinity : _speedLock.min;
            float max = _speedLock.maxInfinity ? Mathf.Infinity : _speedLock.max;
            Mathf.Clamp(PlayerControl.pc.PlayerStats.Speed, min, max);
        }
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