using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IncreasePlayerStatsPassiveItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] PlayerStats _increasingStats;
    PlayerStats _inversedIncreasingStats;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var increaseStatsOriginal = original as IncreasePlayerStatsPassiveItemBehaviour;
        _increasingStats = new(increaseStatsOriginal._increasingStats);
        _inversedIncreasingStats = new(_increasingStats);
        _inversedIncreasingStats.HPRegeneration *= -1;
        _inversedIncreasingStats.Defense *= -1;
        _inversedIncreasingStats.MaxSpeed *= -1;
        _inversedIncreasingStats.MinSpeed *= -1;
        _inversedIncreasingStats.Acceleration *= -1;

        behaviourManager.onPicked += IncreaseStats;

    }
    void IncreaseStats()
    {
        PlayerControl.pc.PlayerStats.IncreaseStats(_increasingStats);
    }
    public override void RemoveBehaviour()
    {
        PlayerControl.pc.PlayerStats.IncreaseStats(_inversedIncreasingStats);
        
    }
}
