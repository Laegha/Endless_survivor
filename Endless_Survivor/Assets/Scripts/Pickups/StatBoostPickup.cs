using AYellowpaper.SerializedCollections;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StatBoost : Pickup
{
    PlayerStats _statsBoost;
    public PlayerStats StatsBoost { set { _statsBoost = value; } }

    public override void PickUp(PlayerControl playerControl)
    {
        base.PickUp(playerControl);

        PlayerStats playerStats = playerControl.PlayerStats;
        playerStats.Damage += _statsBoost.Damage;
        playerStats.AttackSpeed += _statsBoost.AttackSpeed;
        playerStats.Speed += _statsBoost.Speed;
        playerStats.Range += _statsBoost.Range;
        playerStats.MaxHealth += _statsBoost.MaxHealth;
    }
}
