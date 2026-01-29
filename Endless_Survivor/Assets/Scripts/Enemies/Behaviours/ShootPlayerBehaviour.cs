using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class ShootPlayerBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    Transform _player;
    Transform _enemy;
    float _shootCooldown = 0;
    [SerializeField] CustomAnimation _shootingAnimation;
    [SerializeField] int _shootFrame = 0;
    [SerializeField] int _damage = 1;
    [SerializeField] float _atkSpeed = 2.5f;
    [SerializeField] float _proyectileLifetime = 3;
    [SerializeField] Vector2 _firePointPosition = Vector2.zero;
    [SerializeField] ProyectileData _proyectileData;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        ShootPlayerBehaviour originalShootPlayer = original as ShootPlayerBehaviour;
        _shootingAnimation = new CustomAnimation(EnemyControl.Animator, originalShootPlayer._shootingAnimation);
        _shootingAnimation.Events.Add(new(null, _shootFrame, Shoot));
        EnemyControl.Animator.AddAnimations(new List<CustomAnimation> { _shootingAnimation });

        _shootFrame = Mathf.Clamp(originalShootPlayer._shootFrame, 0, _shootingAnimation.Frames.Length -1);
        _damage = originalShootPlayer._damage;
        _atkSpeed = originalShootPlayer._atkSpeed;
        _proyectileLifetime = originalShootPlayer._proyectileLifetime;
        _firePointPosition = originalShootPlayer._firePointPosition;
        _proyectileData = originalShootPlayer._proyectileData;

        _player = PlayerControl.pc.transform;
        _enemy = enemyControl.transform;

    }
    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if (_shootCooldown > 0)
        {
            _shootCooldown -= Time.deltaTime;
        }
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();

        if (_shootCooldown > 0)
            return;

        if (EnemyControl.Animator.CurrAnim == null || EnemyControl.Animator.CurrAnim.AnimationName != _shootingAnimation.AnimationName)
            EnemyControl.Animator.ChangeAnim(_shootingAnimation.AnimationName);

        GameManager.gm.DelayAction(_shootingAnimation.AnimDuration, KillBehaviour, null);
    }
    void Shoot()
    {
        Vector2 distance = _player.position - EnemyControl.transform.position;
        Vector2 shootingOrientation = distance.normalized;
        Vector2 shootingPosition = (Vector2)_enemy.position + _firePointPosition * (shootingOrientation.x > 0 ? 1 : -1);
        Vector2 shootingDirection = ((Vector2)_player.position - shootingPosition).normalized;

        _shootCooldown = 1/ _atkSpeed;
        
        float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        EnemyProyectile proyectile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["EnemyProyectile"], shootingPosition, Quaternion.Euler(0, 0, angle)).GetComponent<EnemyProyectile>();
        proyectile.Initiate(_damage, _proyectileLifetime, _proyectileData);
        
    }
}
