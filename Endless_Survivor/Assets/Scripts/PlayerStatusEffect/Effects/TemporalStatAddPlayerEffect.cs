using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TemporalStatAddPlayerEffect : EndByTimePlayerEffect
{
    new public static int maxStacks => 1;
    [SerializeField] float _addedHpRegen;
    [SerializeField] int _addedMaxHP;
    [SerializeField] float _addedMinSpeed;
    [SerializeField] float _addedMaxSpeed;
    [SerializeField] float _addedAcceleration;
    [SerializeField] float _addedDamageMultiplier;
    public override void Initialize(PlayerStatusEffect original)
    {
        base.Initialize(original);
        var statChangeOriginal = original as TemporalStatAddPlayerEffect;
        _addedHpRegen = statChangeOriginal._addedHpRegen;
        _addedMaxHP = statChangeOriginal._addedMaxHP;
        _addedMinSpeed = statChangeOriginal._addedMinSpeed;
        _addedMaxSpeed = statChangeOriginal._addedMaxSpeed;
        _addedAcceleration = statChangeOriginal._addedAcceleration;
        _addedDamageMultiplier = statChangeOriginal._addedDamageMultiplier;
    }
    public override void Start()
    {
        base.Start();

        PlayerControl.pc.PlayerStats.HPRegeneration += _addedHpRegen;
        PlayerControl.pc.PlayerStats.MinSpeed += _addedMinSpeed;
        PlayerControl.pc.PlayerStats.MaxSpeed += _addedMaxSpeed;
        PlayerControl.pc.PlayerStats.Acceleration += _addedAcceleration;
        PlayerControl.pc.PlayerHPManager.MaxHP += _addedMaxHP;
        PlayerControl.pc.PlayerHPManager.IncomingDamageMultiplier += _addedDamageMultiplier;
    }
    public override void End()
    {
        base.End();

        PlayerControl.pc.PlayerStats.HPRegeneration -= _addedHpRegen;
        PlayerControl.pc.PlayerStats.MinSpeed -= _addedMinSpeed;
        PlayerControl.pc.PlayerStats.MaxSpeed -= _addedMaxSpeed;
        PlayerControl.pc.PlayerStats.Acceleration -= _addedAcceleration;
        PlayerControl.pc.PlayerHPManager.MaxHP -= _addedMaxHP;
        PlayerControl.pc.PlayerHPManager.IncomingDamageMultiplier -= _addedDamageMultiplier;
    }
}
