using System.Collections;
using System.Collections.Generic;
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
        GameManager.gm.StartCoroutine(DelayedAttack());
    }
    IEnumerator DelayedAttack()
    {
        float attackFps = AffectedAttack.ParentWeapon.WeaponControl.WeaponAnimator.Animations.Find(x => x.AnimationName == "Attack").FramesPerSecond;
        float delayTime = _frameDelay / attackFps;
        yield return new WaitForSeconds(delayTime);
        AffectedAttack.ParentWeapon.Attack(_delayedAttackPosOffset, _delayedAttackRotationOffset, true);
    }
}
