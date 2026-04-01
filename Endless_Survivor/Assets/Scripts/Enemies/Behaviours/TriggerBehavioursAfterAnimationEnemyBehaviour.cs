using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerBehavioursAfterAnimationEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _triggeredBehavioursIds;
    [SerializeField] CustomAnimation _upAnimation;
    [SerializeField] CustomAnimation _rightAnimation;
    [SerializeField] CustomAnimation _downAnimation;
    [SerializeField] CustomAnimation _leftAnimation;

    List<CustomAnimation> _animations = new();

    bool _activated = false;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerAfterAnimOriginal = original as TriggerBehavioursAfterAnimationEnemyBehaviour;
        _triggeredBehavioursIds = triggerAfterAnimOriginal._triggeredBehavioursIds;
        if (triggerAfterAnimOriginal._upAnimation.Frames.Length > 0)
        {
            _upAnimation = new CustomAnimation(EnemyControl.Animator, triggerAfterAnimOriginal._upAnimation);
            _upAnimation.Events.Add(new(null, _upAnimation.Frames.Length - 1, TriggerBehaviours));
            _animations.Add(_upAnimation);
        }
        if (triggerAfterAnimOriginal._rightAnimation.Frames.Length > 0)
        {
            _rightAnimation = new CustomAnimation(EnemyControl.Animator, triggerAfterAnimOriginal._rightAnimation);
            _rightAnimation.Events.Add(new(null, _rightAnimation.Frames.Length - 1, TriggerBehaviours));
            _animations.Add(_rightAnimation);

        }
        if (triggerAfterAnimOriginal._downAnimation.Frames.Length > 0)
        {
            _downAnimation = new CustomAnimation(EnemyControl.Animator, triggerAfterAnimOriginal._downAnimation);
            _downAnimation.Events.Add(new(null, _downAnimation.Frames.Length - 1, TriggerBehaviours));
            _animations.Add(_downAnimation);

        }
        if (triggerAfterAnimOriginal._leftAnimation.Frames.Length > 0)
        {
            _leftAnimation = new CustomAnimation(EnemyControl.Animator, triggerAfterAnimOriginal._leftAnimation);
            _leftAnimation.Events.Add(new(null, _leftAnimation.Frames.Length - 1, TriggerBehaviours));
            _animations.Add(_leftAnimation);

        }

        EnemyControl.Animator.AddAnimations(_animations);
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if(!_activated)
        {
            _activated = true;
            Vector2 playerDirection = PlayerControl.pc.transform.position - EnemyControl.transform.position;
            var currAnim = Utility.GetAnimFromDirection(playerDirection, _upAnimation, _rightAnimation, _downAnimation, _leftAnimation);
            EnemyControl.Animator.ChangeAnim(currAnim.AnimationName);
        }
        if (!_animations.Any(x => x.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            TriggerBehaviours();
    }

    void TriggerBehaviours()
    {
        foreach(string id in _triggeredBehavioursIds)
        {
            EnemyControl.BehaviourManager.ActivateBehaviour(id);
        }
        if(_animations.Any(x => x.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);
        KillBehaviour();
    }

    public override void KillBehaviour()
    {
        base.KillBehaviour();
        _activated = false;
    }
}
