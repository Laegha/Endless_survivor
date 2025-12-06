using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class ShootSecondDelayedAttack : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _frameDelay;
    [SerializeField] Vector2 _delayedAttackPosOffset;
    [SerializeField] float _delayedAttackRotationOffset;
    public ShootSecondDelayedAttack(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var delayedAttackOriginal = original as ShootSecondDelayedAttack;
        _frameDelay = delayedAttackOriginal._frameDelay;
        _delayedAttackPosOffset = delayedAttackOriginal._delayedAttackPosOffset;
        _delayedAttackRotationOffset = delayedAttackOriginal._delayedAttackRotationOffset;
        OnAttack += ThrowDelayedAttack;
    }
    void ThrowDelayedAttack()
    {
        float attackFps = AffectedAttack.ParentWeapon.WeaponControl.WeaponAnimator.Animations.Find(x => x.AnimationName == AffectedAttack.ParentWeapon.AnimationName).FramesPerSecond;
        float delayTime = _frameDelay / attackFps;
        GameManager.gm.DelayAction(delayTime, DelayedAttack, () => AffectedAttack.ParentWeapon == null);
    }
    void DelayedAttack()
    {
        AffectedAttack.ParentWeapon.Attack(_delayedAttackPosOffset, _delayedAttackRotationOffset, true);
    }
}
