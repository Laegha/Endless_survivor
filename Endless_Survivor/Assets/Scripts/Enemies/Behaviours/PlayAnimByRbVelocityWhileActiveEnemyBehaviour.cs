using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayAnimByRbVelocityWhileActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] DirectionalCustomAnimation _animations;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var playAnimOriginal = original as PlayAnimByRbVelocityWhileActiveEnemyBehaviour;
        _animations = new (EnemyControl.Animator, playAnimOriginal._animations);
        EnemyControl.Animator.AddAnimations(_animations.NonNullAnimations);
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        Vector2 direction = EnemyControl.RbForcesController.Rb.velocity.normalized;
        if (direction == Vector2.zero)
        {
            EndAnimation();
            return;
        }
        var currAnim = _animations.GetAnim(direction);
        if (EnemyControl.Animator.CurrAnim.AnimationName != currAnim.AnimationName && _animations.NonNullAnimations.Any(anim => anim.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim);
        EnemyControl.Animator.ChangeAnim(currAnim.AnimationName);

    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        EndAnimation();
    }

    void EndAnimation()
    {
        if (_animations.NonNullAnimations.Any(anim => anim.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);

    }


}
