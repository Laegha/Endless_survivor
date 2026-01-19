using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DashTowardsPlayerEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] float _dashSpeed;
    [SerializeField] float _dashDistance;
    [SerializeField] int _dashForcePriority = 5;
    [SerializeField] CustomAnimation _dashAnimation;
    [SerializeField] ParticleSystem _dashParticles;
    [SerializeField] float _particlesDuration;
    [SerializeField] bool _particlesFollowEnemy;

    Vector2 _direction;
    bool _isActivated;
    float _lapsedDistance;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var dashOriginal = original as DashTowardsPlayerEnemyBehaviour;
        _dashSpeed = dashOriginal._dashSpeed;
        _dashDistance = dashOriginal._dashDistance;
        _dashForcePriority = dashOriginal._dashForcePriority;
        _dashAnimation = new(EnemyControl.Animator, dashOriginal._dashAnimation);
        EnemyControl.Animator.AddAnimations(new (){ _dashAnimation });
        _dashParticles = dashOriginal._dashParticles;
        _particlesDuration = dashOriginal._particlesDuration;
        _particlesFollowEnemy = dashOriginal._particlesFollowEnemy;
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
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
        }
        if (_dashAnimation.Frames.Length > 0 && EnemyControl.Animator.CurrAnim == null || EnemyControl.Animator.CurrAnim.AnimationName != _dashAnimation.AnimationName)
            EnemyControl.Animator.ChangeAnim(_dashAnimation.AnimationName);

        EnemyControl.RbForcesController.ChangeCurrForce(new(_direction.normalized, _dashSpeed, _dashForcePriority, ForceMode2D.Force));
        _lapsedDistance += Time.deltaTime * _dashSpeed;
        if(_lapsedDistance > _dashDistance)
        {
            _lapsedDistance -= _dashSpeed;//this is horrible, god forbid me. i set the distance back so it doesn't kill the behaviour again in the delay frame
            KillBehaviour();
        }
    }

    public override void KillBehaviour()
    {
        base.KillBehaviour();
        _isActivated = false;
        EnemyControl.RbForcesController.Stop();

    }
}
