using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayAnimFacingPlayerWhileActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] DirectionalCustomAnimation _animations;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var playAnimOriginal = original as PlayAnimFacingPlayerWhileActiveEnemyBehaviour;
        _animations = new(EnemyControl.Animator, playAnimOriginal._animations);
        EnemyControl.Animator.AddAnimations(_animations.NonNullAnimations);
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        Vector2 direction = (PlayerControl.pc.transform.position - EnemyControl.transform.position).normalized;
        var currAnim = _animations.GetAnim(direction);
        if (EnemyControl.Animator.CurrAnim!= null && EnemyControl.Animator.CurrAnim.AnimationName != currAnim.AnimationName && _animations.NonNullAnimations.Any(anim => anim.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim);
        EnemyControl.Animator.ChangeAnim(currAnim.AnimationName);

    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        if(_animations.NonNullAnimations.Any(anim => anim.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName)) 
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);
    }
}
