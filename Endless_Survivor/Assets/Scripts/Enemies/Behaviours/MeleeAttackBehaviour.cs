using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MeleeAttackBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    Transform _player;
    Transform _enemy;
    bool _attacked = false;
    Vector2 _attackDirection;


    [SerializeField] int _damage = 2;
    [SerializeField] float _attackRange = 1;
    [SerializeField] float _attackRadius = 1;

    [SerializeField] CustomAnimation _attackUpAnimation;
    [SerializeField] CustomAnimation _attackRightAnimation;
    [SerializeField] CustomAnimation _attackDownAnimation;
    [SerializeField] CustomAnimation _attackLeftAnimation;

    [SerializeField] bool _usesVfx;
    [SerializeField] CustomAnimation _attackVFXAnimation;
    [SerializeField] int _triggerVFXFrame;
    [SerializeField] int _vfxRendererOffset;
    [SerializeField] bool _vfxUsesFixedAngle;
    [SerializeField] float _vfxFixedAngle;
    [Tooltip("If there are multiple animations, make sure that this frame isn't out of range for any of them")][SerializeField] int _triggerDamageFrame;
    [SerializeField] SFXInfo _attackSfx;
    CustomAnimator _vfxAnimator;


    List<CustomAnimation> _attackAnimations = new();
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        MeleeAttackBehaviour originalMeleeAttack = original as MeleeAttackBehaviour;
        _player = PlayerControl.pc.transform;
        _enemy = enemyControl.transform;

        _damage = originalMeleeAttack._damage;
        _attackRange = originalMeleeAttack._attackRange;
        _attackRadius = originalMeleeAttack._attackRadius;
        _usesVfx = originalMeleeAttack._usesVfx;

        //copy animations from SO
        if (originalMeleeAttack._attackUpAnimation.Frames.Length > 0)
        {
            _attackUpAnimation = new CustomAnimation(EnemyControl.Animator, originalMeleeAttack._attackUpAnimation);

            _attackUpAnimation.Events.Add(new(null, _attackUpAnimation.Frames.Length - 1, AnimationEnd));
            _attackAnimations.Add(_attackUpAnimation);
        }
        if (originalMeleeAttack._attackRightAnimation.Frames.Length > 0)
        {
            _attackRightAnimation = new CustomAnimation(EnemyControl.Animator, originalMeleeAttack._attackRightAnimation);

            _attackRightAnimation.Events.Add(new(null, _attackRightAnimation.Frames.Length - 1, AnimationEnd));
            _attackAnimations.Add(_attackRightAnimation);

        }
        if (originalMeleeAttack._attackDownAnimation.Frames.Length > 0)
        {
            _attackDownAnimation = new CustomAnimation(EnemyControl.Animator, originalMeleeAttack._attackDownAnimation);

            _attackDownAnimation.Events.Add(new(null, _attackDownAnimation.Frames.Length - 1, AnimationEnd));
            _attackAnimations.Add(_attackDownAnimation);

        }
        if (originalMeleeAttack._attackLeftAnimation.Frames.Length > 0)
        {
            _attackLeftAnimation = new CustomAnimation(EnemyControl.Animator, originalMeleeAttack._attackLeftAnimation);

            _attackLeftAnimation.Events.Add(new(null, _attackLeftAnimation.Frames.Length - 1, AnimationEnd));
            _attackAnimations.Add(_attackLeftAnimation);

        }
        _triggerDamageFrame = originalMeleeAttack._triggerDamageFrame;
        _attackSfx = originalMeleeAttack._attackSfx;


        if (_usesVfx)
        {
            _triggerVFXFrame = originalMeleeAttack._triggerVFXFrame;
            _vfxRendererOffset = originalMeleeAttack._vfxRendererOffset;
            _vfxUsesFixedAngle = originalMeleeAttack._vfxUsesFixedAngle;
            _vfxFixedAngle = originalMeleeAttack._vfxFixedAngle;
        
            _vfxAnimator = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["AnimatedObject"], enemyControl.transform).GetComponent<CustomAnimator>();
            _vfxAnimator.GetComponent<RendererSortingByY>().Offset = _vfxRendererOffset;
            _attackVFXAnimation = new CustomAnimation(_vfxAnimator, originalMeleeAttack._attackVFXAnimation);

            foreach (var anim in _attackAnimations)
            {
                anim.Events.Add(new(null, _triggerVFXFrame, TriggerAttackVFX));

            }
            _attackVFXAnimation.Events.Add(new(null, _triggerDamageFrame, TriggerDamage));
            _attackVFXAnimation.Events.Add(new(null, _attackVFXAnimation.Frames.Length - 1, () =>
            {
                _vfxAnimator.EndAnimation(_attackVFXAnimation.AnimationName);
                _vfxAnimator.ChangeAnim("Idle");
            }));


            CustomAnimation vfxIdleAnim = new CustomAnimation(null, "Idle", new Sprite[1]);
            _vfxAnimator.AddAnimations(new List<CustomAnimation> { vfxIdleAnim, _attackVFXAnimation });
        }
        else
        {
            foreach (var anim in _attackAnimations)
            {
                anim.Events.Add(new(null, _triggerDamageFrame, TriggerDamage));

            }

        }

        EnemyControl.Animator.AddAnimations(_attackAnimations);
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();

        if (_attacked)
            return;
        _attackDirection = ((Vector2)(_player.position - _enemy.position)).normalized;
        _attacked = true;
        Attack();

    }
    void Attack()
    {
        CustomAnimation currAnim = Utility.GetAnimFromDirection(_attackDirection, _attackUpAnimation, _attackRightAnimation, _attackDownAnimation, _attackLeftAnimation);
        if (EnemyControl.Animator.CurrAnim == null || EnemyControl.Animator.CurrAnim.AnimationName != currAnim.AnimationName)
        {
            if (_attackAnimations.Any(x => x.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            {
                EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);
            }
            EnemyControl.Animator.ChangeAnim(currAnim.AnimationName);
        }
    }
    void TriggerAttackVFX()
    {
        SoundFXManager.sm.PlaySfx(_attackSfx, EnemyControl.transform.position);
        _vfxAnimator.transform.localPosition = _attackDirection * _attackRange;
        float angle = _vfxUsesFixedAngle ? _vfxFixedAngle : Mathf.Atan2(_attackDirection.y, _attackDirection.x) * Mathf.Rad2Deg;
        _vfxAnimator.transform.rotation = Quaternion.Euler(0, 0, angle);

        _vfxAnimator.ChangeAnim(_attackVFXAnimation.AnimationName);
    }
    void AnimationEnd()
    {
        KillBehaviour();
    }
    void TriggerDamage()
    {
        Debug.Log("Damaging on an area");
        //trigger an animation
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(_attackDirection * _attackRange + (Vector2)_enemy.position, _attackRadius);

        foreach (Collider2D collider in hitColliders)
        {
            if(collider.CompareTag("Player"))
            {
                PlayerControl.pc.PlayerHPManager.TakeDamage(_damage);
                return;
            }
        }
    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        if(_attackAnimations.Any(x => x.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);
        _attacked = false;
    }
}
