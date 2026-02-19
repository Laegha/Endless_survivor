using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayIdleAnimFacingPlayerEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => 1;
    [SerializeField] CustomAnimation _idleUpAnimation;
    [SerializeField] CustomAnimation _idleRightAnimation;
    [SerializeField] CustomAnimation _idleDownAnimation;
    [SerializeField] CustomAnimation _idleLeftAnimation;

    List<CustomAnimation> _idleAnims = new List<CustomAnimation>();

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var originalPlayIdle = original as PlayIdleAnimFacingPlayerEnemyBehaviour;
        if (originalPlayIdle._idleUpAnimation.Frames.Length > 0)
        {
            _idleUpAnimation = new CustomAnimation(EnemyControl.Animator, originalPlayIdle._idleUpAnimation);
            _idleAnims.Add(_idleUpAnimation);
        }
        if (originalPlayIdle._idleRightAnimation.Frames.Length > 0)
        {
            _idleRightAnimation = new CustomAnimation(EnemyControl.Animator, originalPlayIdle._idleRightAnimation);
            _idleAnims.Add(_idleRightAnimation);

        }
        if (originalPlayIdle._idleDownAnimation.Frames.Length > 0)
        {
            _idleDownAnimation = new CustomAnimation(EnemyControl.Animator, originalPlayIdle._idleDownAnimation);
            _idleAnims.Add(_idleDownAnimation);

        }
        if (originalPlayIdle._idleLeftAnimation.Frames.Length > 0)
        {
            _idleLeftAnimation = new CustomAnimation(EnemyControl.Animator, originalPlayIdle._idleLeftAnimation);
            _idleAnims.Add(_idleLeftAnimation);

        }
        EnemyControl.Animator.AddAnimations(_idleAnims);



    }

    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        Vector2 playerDir = (PlayerControl.pc.transform.position - EnemyControl.transform.position).normalized;

        var currAnim = Utility.GetAnimFromDirection(playerDir, _idleUpAnimation, _idleRightAnimation, _idleDownAnimation, _idleLeftAnimation);

        EnemyControl.Animator.ChangeAnim(currAnim.AnimationName); 

    }
}
