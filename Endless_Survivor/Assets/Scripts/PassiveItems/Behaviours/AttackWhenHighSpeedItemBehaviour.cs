using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class AttackWhenHighSpeedItemBehaviour : PassiveItemBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] float _speedThreshold;
    [SerializeField] float _speedTop;
    [SerializeField] CustomAnimation _rightAnimation;
    [SerializeField] CustomAnimation _leftAnimation;
    [SerializeField] int _attackFrame;
    [SerializeField] Vector2 _attackOffset;
    [SerializeField] WeaponStats _attackBaseStats;
    [SerializeField] WeaponStats _statsScaling;
    [SerializeField] DamageInfo.DamageType _damageType;
    [SerializeField] MeleeData _attackData;

    WeaponStats _attackStats;
    float _playerSpeed => PlayerControl.pc.PlayerRb.velocity.magnitude;
    DamageInfo DamageInfo => new DamageInfo(_attackStats.Damage, _damageType);
    float Damage => DamageInfo.CalculatedDamage;
    public override void CopyValues(PassiveItemBehaviour original, PassiveItemBehaviourManager behaviourManager)
    {
        base.CopyValues(original, behaviourManager);
        var attackWhenHighSpeedOriginal = original as AttackWhenHighSpeedItemBehaviour;
        _speedThreshold = attackWhenHighSpeedOriginal._speedThreshold;
        _speedTop = attackWhenHighSpeedOriginal._speedTop;
        _rightAnimation = new(PlayerControl.pc.PlayerAnimator, attackWhenHighSpeedOriginal._rightAnimation);
        _leftAnimation = new(PlayerControl.pc.PlayerAnimator,  attackWhenHighSpeedOriginal._leftAnimation);
        _attackOffset = attackWhenHighSpeedOriginal._attackOffset;
        _attackBaseStats = attackWhenHighSpeedOriginal._attackBaseStats;
        _statsScaling = attackWhenHighSpeedOriginal._statsScaling;
        _damageType = attackWhenHighSpeedOriginal._damageType;
        _attackData = attackWhenHighSpeedOriginal._attackData;

        _rightAnimation.Events.Add(new(null, _attackFrame, Attack));
        _leftAnimation.Events.Add(new(null, _attackFrame, Attack));

        PlayerControl.pc.PlayerAnimator.AddAnimations(new(){ _rightAnimation, _leftAnimation });
        
        UpdateStats();
        WaveManager.wm.OnWaveStarted += UpdateStats;
        behaviourManager.onUpdate += CheckSpeed;
    }
    void CheckSpeed()
    {
        Debug.Log(_attackStats.Damage);
        if(_playerSpeed < _speedThreshold || _playerSpeed > _speedTop)
        {
            var currAnimName = PlayerControl.pc.PlayerAnimator.CurrAnim.AnimationName;
            if (currAnimName == _rightAnimation.AnimationName || currAnimName == _leftAnimation.AnimationName)
                PlayerControl.pc.PlayerAnimator.EndAnimation(currAnimName);
            return;
        }
        var xDir = PlayerControl.pc.PlayerRb.velocity.x;
        //Attack();
        PlayerControl.pc.PlayerAnimator.ChangeAnim(xDir > 0 ? _rightAnimation.AnimationName : _leftAnimation.AnimationName);
    }
    void Attack()
    {
        var attackDir = PlayerControl.pc.PlayerRb.velocity.x > 0 ? 1 : -1;
        var meleeAttack = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Melee"]).GetComponent<MeleeAttack>();
        meleeAttack.transform.SetParent(PlayerControl.pc.transform);
        meleeAttack.transform.localPosition = new(_attackOffset.x * attackDir, _attackOffset.y);
        meleeAttack.StartAttack((int)Damage, _attackBaseStats.Knockback, _attackData);
        meleeAttack.ApplyDamage();
        GameObject.Destroy(meleeAttack.gameObject, 1f);
    }
    void UpdateStats()
    {
        var weaponStats = new WeaponStats(_attackBaseStats);
        weaponStats.SetTrueLevelStats(_statsScaling, ScalingFunctions.CurrWaveLevel);
        _attackStats = weaponStats;
    }
}
