using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TriggerBehavioursOnAnimFrameEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] string[] _triggeredBehaviours;
    [SerializeField] bool _loopAnim;
    [Tooltip("If true, all animations use the general frame. Else, each one uses the specific")][SerializeField] bool _sameFrameForAll;
    [Tooltip("Use if sameForAll is true")][SerializeField] int _generalTriggerFrame;
    [SerializeField] CustomAnimation _upAnimation;
    [SerializeField] int _upTriggerFrame;
    [SerializeField] CustomAnimation _rightAnimation;
    [SerializeField] int _rightTriggerFrame;
    [SerializeField] CustomAnimation _downAnimation;
    [SerializeField] int _downTriggerFrame;
    [SerializeField] CustomAnimation _leftAnimation;
    [SerializeField] int _leftTriggerFrame;

    CustomAnimation _currAnim = null;
    List<CustomAnimation> _animations = new();
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var triggerOnAnimFrameOriginal = original as TriggerBehavioursOnAnimFrameEnemyBehaviour;
        _triggeredBehaviours = triggerOnAnimFrameOriginal._triggeredBehaviours;

        _loopAnim = triggerOnAnimFrameOriginal._loopAnim;

        _sameFrameForAll = triggerOnAnimFrameOriginal._sameFrameForAll;
        _generalTriggerFrame = triggerOnAnimFrameOriginal._generalTriggerFrame;
        _upTriggerFrame = triggerOnAnimFrameOriginal._upTriggerFrame;
        _rightTriggerFrame = triggerOnAnimFrameOriginal._rightTriggerFrame;
        _downTriggerFrame = triggerOnAnimFrameOriginal._downTriggerFrame;
        _leftTriggerFrame = triggerOnAnimFrameOriginal._leftTriggerFrame;

        if (triggerOnAnimFrameOriginal._upAnimation.Frames.Length > 0)
        {
            _upAnimation = new CustomAnimation(EnemyControl.Animator, triggerOnAnimFrameOriginal._upAnimation);
            _upAnimation.Events.Add(new(null, _sameFrameForAll ? _generalTriggerFrame : _upTriggerFrame, TriggerBehaviours));
            _animations.Add(_upAnimation);
        }
        if (triggerOnAnimFrameOriginal._rightAnimation.Frames.Length > 0)
        {
            _rightAnimation = new CustomAnimation(EnemyControl.Animator, triggerOnAnimFrameOriginal._rightAnimation);
            _rightAnimation.Events.Add(new(null, _sameFrameForAll ? _generalTriggerFrame : _rightTriggerFrame, TriggerBehaviours));
            _animations.Add(_rightAnimation);

        }
        if (triggerOnAnimFrameOriginal._downAnimation.Frames.Length > 0)
        {
            _downAnimation = new CustomAnimation(EnemyControl.Animator, triggerOnAnimFrameOriginal._downAnimation);
            _downAnimation.Events.Add(new(null, _sameFrameForAll ? _generalTriggerFrame : _downTriggerFrame, TriggerBehaviours));
            _animations.Add(_downAnimation);

        }
        if (triggerOnAnimFrameOriginal._leftAnimation.Frames.Length > 0)
        {
            _leftAnimation = new CustomAnimation(EnemyControl.Animator, triggerOnAnimFrameOriginal._leftAnimation);
            _leftAnimation.Events.Add(new(null, _sameFrameForAll ? _generalTriggerFrame : _leftTriggerFrame, TriggerBehaviours));
            _animations.Add(_leftAnimation);

        }
        
        EnemyControl.Animator.AddAnimations(_animations);
    }

    void TriggerBehaviours()
    {
        foreach (var behaviourId in _triggeredBehaviours)
        {
            EnemyControl.BehaviourManager.ActivateBehaviour(behaviourId);
        }
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();

        if (_currAnim != null/* && _currAnim == EnemyControl.Animator.CurrAnim*/)
        {
            return;
        }
        Vector2 distance = PlayerControl.pc.transform.position - EnemyControl.transform.position;
        Vector2 orientation = distance.normalized;

        CustomAnimation currAnim = Utility.GetAnimFromDirection(orientation, _upAnimation, _rightAnimation, _downAnimation, _leftAnimation);

        if (EnemyControl.Animator.CurrAnim != null && _animations.Any(x => x.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);
        _currAnim = currAnim;
        if(!_loopAnim)
        {
            GameManager.gm.DelayAction(currAnim.AnimDuration, () =>
            {
                KillBehaviour();
            }, null);

        }
        EnemyControl.Animator.ChangeAnim(currAnim.AnimationName);
    }

    public override void KillBehaviour()
    {
        base.KillBehaviour();
        _currAnim = null;
        if (_animations.Any(x => x.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);
    }
}
