using System.Collections;
using System.Collections.Generic;
using System.Net.Security;
using UnityEngine;

public class ShootPlayerBehaviour : EnemyBehaviour
{
    Transform _player;
    Transform _enemy;
    float _shootCooldown = 0;
    [SerializeField] CustomAnimation _shootingAnimation;
    [SerializeField] float _distanceToStopShooting = 4;
    [SerializeField] int _damage = 1;
    [SerializeField] float _atkSpeed = 2.5f;
    [SerializeField] float _proyectileSpeed = 1.5f;
    [SerializeField] float _proyectileSpread = 10;
    [SerializeField] float _proyectileLifetime = 3;
    [SerializeField] Vector2 _firePointPosition = Vector2.zero;
    [SerializeField] ProyectileData _proyectileData;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        ShootPlayerBehaviour originalShootPlayer = original as ShootPlayerBehaviour;
        _shootingAnimation = new CustomAnimation(EnemyControl.Animator, originalShootPlayer._shootingAnimation);
        EnemyControl.Animator.AddAnimations(new List<CustomAnimation> { _shootingAnimation });

        _distanceToStopShooting = originalShootPlayer._distanceToStopShooting;
        _damage = originalShootPlayer._damage;
        _atkSpeed = originalShootPlayer._atkSpeed;
        _proyectileSpeed = originalShootPlayer._proyectileSpeed;
        _proyectileSpread = originalShootPlayer._proyectileSpread;
        _proyectileLifetime = originalShootPlayer._proyectileLifetime;
        _firePointPosition = originalShootPlayer._firePointPosition;
        _proyectileData = originalShootPlayer._proyectileData;

        _player = PlayerControl.pc.transform;
        _enemy = enemyControl.transform;

    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate(); 
        if (EnemyControl.Animator.CurrAnim == null || EnemyControl.Animator.CurrAnim.AnimationName != _shootingAnimation.AnimationName)
            EnemyControl.Animator.ChangeAnim(_shootingAnimation.AnimationName);
        if (_shootCooldown > 0)
        {
            _shootCooldown -= Time.deltaTime;
            return;
        }
        Vector2 distance = _player.position - EnemyControl.transform.position;
        if(distance.magnitude >= _distanceToStopShooting)
        {
            KillBehaviour();
            return;
        }
        Vector2 shootingOrientation = distance.normalized;
        Vector2 shootingPosition = (Vector2)_enemy.position + _firePointPosition * (shootingOrientation.x > 0 ? 1 : -1);
        Vector2 shootingDirection = ((Vector2)_player.position - shootingPosition).normalized;

        RaycastHit2D hit = Physics2D.Raycast(shootingPosition, shootingDirection);
        //if (!hit.collider.CompareTag("Player"))//Add layermask
        //{
        //    KillBehaviour();
        //    return;
        //}

        Shoot(shootingPosition, shootingDirection);
    }
    void Shoot(Vector2 shootingPosition, Vector2 shootingDirection)
    {
        Debug.Log("Enemy Shooting");
        _shootCooldown = 1/ _atkSpeed;
        float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        Proyectile proyectile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["EnemyProyectile"], shootingPosition, Quaternion.Euler(0, 0, angle)).GetComponent<Proyectile>();
        proyectile.Initiate(_damage,0, _proyectileSpeed, _proyectileLifetime, _proyectileData, _proyectileSpread);
    }
}
