using System.Collections;
using System.Collections.Generic;
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

    [SerializeField] CustomAnimation _enemyAttackAnimation;
    [SerializeField] bool _usesVfx;
    [SerializeField] ChangeOnEndAnimation _attackVFXAnimation;
    [SerializeField] AnimationEvent _triggerVFXEvent;
    [SerializeField] AnimationEvent _triggerDamageEvent;
    [SerializeField] SFXInfo _attackSfx;
    CustomAnimator _vfxAnimator;
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
        _enemyAttackAnimation = new CustomAnimation(EnemyControl.Animator, originalMeleeAttack._enemyAttackAnimation);
        _attackVFXAnimation = new ChangeOnEndAnimation(_vfxAnimator, originalMeleeAttack._attackVFXAnimation);
        _triggerDamageEvent = new AnimationEvent(originalMeleeAttack._triggerDamageEvent);
        _attackSfx = originalMeleeAttack._attackSfx;

        _enemyAttackAnimation.Events.Add(new(null, _enemyAttackAnimation.Frames.Length - 1, AnimationEnd));
        _triggerDamageEvent.frameAction += TriggerDamage;

        if (_usesVfx)
        {
            _triggerVFXEvent = new AnimationEvent(originalMeleeAttack._triggerVFXEvent);
        
            _vfxAnimator = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["AnimatedObject"], enemyControl.transform).GetComponent<CustomAnimator>();
            
            _triggerVFXEvent.frameAction += TriggerAttackVFX;
            _enemyAttackAnimation.Events.Add(_triggerVFXEvent);
            _attackVFXAnimation.Events.Add(_triggerDamageEvent);
            
            CustomAnimation vfxIdleAnim = new CustomAnimation(null, "Idle", new Sprite[1]);
            _vfxAnimator.AddAnimations(new List<CustomAnimation> { vfxIdleAnim, _attackVFXAnimation });
        }
        else
        {
            _enemyAttackAnimation.Events.Add(_triggerDamageEvent);

        }

        EnemyControl.Animator.AddAnimations(new List<CustomAnimation> { _enemyAttackAnimation });
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
        EnemyControl.Animator.ChangeAnim(_enemyAttackAnimation.AnimationName);
    }
    void TriggerAttackVFX()
    {
        SoundFXManager.sm.PlaySfx(_attackSfx, EnemyControl.transform.position);
        _vfxAnimator.transform.localPosition = _attackDirection * _attackRange;
        float angle = Mathf.Atan2(_attackDirection.y, _attackDirection.x) * Mathf.Rad2Deg;
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
        EnemyControl.Animator.EndAnimation(_enemyAttackAnimation);
        _attacked = false;
    }
}
