using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DashTowardsPlayerEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDistance;
    [SerializeField] int _dashForcePriority = 5;
    [SerializeField] DirectionalCustomAnimation _dashAnimations;
    [SerializeField] ParticleSystem _dashParticles;
    [SerializeField] float _particlesDuration;
    [SerializeField] bool _particlesFollowEnemy;
    [SerializeField] SFXInfo _onDashSFX;

    Vector2 _direction;
    bool _isActivated;
    bool _inDelayedFrame;
    float _lapsedDistance;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var dashOriginal = original as DashTowardsPlayerEnemyBehaviour;
        _dashSpeed = dashOriginal._dashSpeed;
        _dashDistance = dashOriginal._dashDistance;
        _dashForcePriority = dashOriginal._dashForcePriority;
        _dashAnimations = new(EnemyControl.Animator, dashOriginal._dashAnimations);
        EnemyControl.Animator.AddAnimations(_dashAnimations.NonNullAnimations);
        _dashParticles = dashOriginal._dashParticles;
        _particlesDuration = dashOriginal._particlesDuration;
        _particlesFollowEnemy = dashOriginal._particlesFollowEnemy;
        _onDashSFX = dashOriginal._onDashSFX;
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        if (_inDelayedFrame)
        {
            _inDelayedFrame = false;
            return;
        }
        if(! _isActivated )
        {
            _isActivated = true;
            _lapsedDistance = 0;
            _direction = PlayerControl.pc.transform.position - EnemyControl.transform.position;
            if (_dashParticles != null)
            {
                ParticleConfig particleConfig = new(_dashParticles, EnemyControl.transform.position, Quaternion.identity, _particlesDuration, _particlesFollowEnemy ? EnemyControl.transform : null, _particlesFollowEnemy, _particlesFollowEnemy);
                ParticleManager.pm.SpawnParticles(particleConfig);
            }
            SoundFXManager.sm.PlaySfx(_onDashSFX, EnemyControl.transform.position);
        }
        if (_dashAnimations.GetAnim(_direction).Frames.Length > 0 && EnemyControl.Animator.CurrAnim == null || EnemyControl.Animator.CurrAnim.AnimationName != _dashAnimations.GetAnim(_direction).AnimationName)
            EnemyControl.Animator.ChangeAnim(_dashAnimations.GetAnim(_direction).AnimationName);

        EnemyControl.RbForcesController.ChangeCurrForce(new(_direction.normalized, _dashSpeed, _dashForcePriority, ForceMode2D.Force));
        _lapsedDistance += Time.deltaTime * _dashSpeed;
        if(_lapsedDistance > _dashDistance)
        {
            _inDelayedFrame = true;
            KillBehaviour();
        }
    }

    public override void KillBehaviour()
    {
        base.KillBehaviour();
        if (_dashAnimations.NonNullAnimations.Any(anim => anim.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);

        _isActivated = false;
        EnemyControl.RbForcesController.Stop();

    }
}
