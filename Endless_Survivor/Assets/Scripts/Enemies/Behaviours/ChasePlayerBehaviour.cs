using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class ChasePlayerBehaviour : EnemyBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] float _enemySpeed;
    [SerializeField] CustomAnimation _chasingUpAnimation;
    [SerializeField] CustomAnimation _chasingRightAnimation;
    [SerializeField] CustomAnimation _chasingDownAnimation;
    [SerializeField] CustomAnimation _chasingLeftAnimation;
    
    List<CustomAnimation> _chasingAnimations = new List<CustomAnimation>();
    Transform _player;

    bool _stopped;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);

        ChasePlayerBehaviour originalChasePlayer = original as ChasePlayerBehaviour;
        _enemySpeed = originalChasePlayer._enemySpeed;

        if (originalChasePlayer._chasingUpAnimation.Frames.Length > 0)
        {
            _chasingUpAnimation = new CustomAnimation(EnemyControl.Animator, originalChasePlayer._chasingUpAnimation);
            _chasingAnimations.Add(_chasingUpAnimation);
        }
        if (originalChasePlayer._chasingRightAnimation.Frames.Length > 0)
        {
            _chasingRightAnimation = new CustomAnimation(EnemyControl.Animator, originalChasePlayer._chasingRightAnimation);
            _chasingAnimations.Add(_chasingRightAnimation);

        }
        if (originalChasePlayer._chasingDownAnimation.Frames.Length > 0)
        {
            _chasingDownAnimation = new CustomAnimation(EnemyControl.Animator, originalChasePlayer._chasingDownAnimation);
            _chasingAnimations.Add(_chasingDownAnimation);

        }
        if (originalChasePlayer._chasingLeftAnimation.Frames.Length > 0)
        {
            _chasingLeftAnimation = new CustomAnimation(EnemyControl.Animator, originalChasePlayer._chasingLeftAnimation);
            _chasingAnimations.Add(_chasingLeftAnimation);

        }

        EnemyControl.Animator.AddAnimations(_chasingAnimations);

        _player = GameObject.FindObjectOfType<PlayerControl>().transform;
    }

    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if(!IsActive)
            _stopped = !EnemyControl.BehaviourManager.ActivateBehaviour(this);
    }

    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if (_stopped)
            return;
        Vector2 direction = (_player.position - EnemyControl.transform.position).normalized;

        CustomAnimation currAnim = Utility.GetAnimFromDirection(direction, _chasingUpAnimation, _chasingRightAnimation, _chasingDownAnimation, _chasingLeftAnimation);
        if (EnemyControl.Animator.CurrAnim == null || EnemyControl.Animator.CurrAnim.AnimationName != currAnim.AnimationName)
        {
            if (_chasingAnimations.Any(x => x.AnimationName == EnemyControl.Animator.CurrAnim?.AnimationName))
            {
                EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);
            }
            EnemyControl.Animator.ChangeAnim(currAnim.AnimationName);
        }


        EnemyControl.RbForcesController.ChangeCurrForce(new(direction, _enemySpeed, 0, ForceMode2D.Force));
    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        _stopped = true;
        EnemyControl.RbForcesController.Stop();
    }
}
