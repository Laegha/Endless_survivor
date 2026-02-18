using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShootPlayerWhileActiveEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;

    [SerializeField] CustomAnimation _shootingUpAnimation;
    [SerializeField] CustomAnimation _shootingRightAnimation;
    [SerializeField] CustomAnimation _shootingDownAnimation;
    [SerializeField] CustomAnimation _shootingLeftAnimation;

    [SerializeField] int _damage = 1;
    [SerializeField] float _atkSpeed = 2.5f;
    [SerializeField] float _proyectileLifetime = 3;
    [SerializeField] float _firePointDist = 1.5f;
    [SerializeField] float _proyectileRotationOffset;

    [SerializeField] ProyectileData _proyectileData;

    Transform _player;
    Transform _enemy;
    float _shootCooldown = 0;

    List<CustomAnimation> _shootingAnimations = new();
    CustomAnimation _currAnim;
    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        ShootPlayerWhileActiveEnemyBehaviour originalShootPlayer = original as ShootPlayerWhileActiveEnemyBehaviour;
        if(originalShootPlayer._shootingUpAnimation.Frames.Length > 0)
        {
            _shootingUpAnimation = new CustomAnimation(EnemyControl.Animator, originalShootPlayer._shootingUpAnimation);
            _shootingAnimations.Add(_shootingUpAnimation);
        }
        if(originalShootPlayer._shootingRightAnimation.Frames.Length > 0)
        {
            _shootingRightAnimation = new CustomAnimation(EnemyControl.Animator, originalShootPlayer._shootingRightAnimation);
            _shootingAnimations.Add(_shootingRightAnimation);
            
        }
        if(originalShootPlayer._shootingDownAnimation.Frames.Length > 0)
        {
            _shootingDownAnimation = new CustomAnimation(EnemyControl.Animator, originalShootPlayer._shootingDownAnimation);
            _shootingAnimations.Add(_shootingDownAnimation);
            
        }
        if(originalShootPlayer._shootingLeftAnimation.Frames.Length > 0)
        {
            _shootingLeftAnimation = new CustomAnimation(EnemyControl.Animator, originalShootPlayer._shootingLeftAnimation);
            _shootingAnimations.Add(_shootingLeftAnimation);
            
        }

        EnemyControl.Animator.AddAnimations(_shootingAnimations);

        _damage = originalShootPlayer._damage;
        _atkSpeed = originalShootPlayer._atkSpeed;
        _proyectileLifetime = originalShootPlayer._proyectileLifetime;
        _firePointDist = originalShootPlayer._firePointDist;
        _proyectileRotationOffset = originalShootPlayer._proyectileRotationOffset;


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
        Vector2 distance = _player.position - EnemyControl.transform.position;
        Vector2 shootingOrientation = distance.normalized;

        CustomAnimation currAnim = Utility.GetAnimFromDirection(shootingOrientation, _shootingUpAnimation, _shootingRightAnimation, _shootingDownAnimation, _shootingLeftAnimation);
        _currAnim = currAnim;
        if (EnemyControl.Animator.CurrAnim == null || EnemyControl.Animator.CurrAnim.AnimationName != currAnim.AnimationName)
        {
            if(_shootingAnimations.Any(x => x.AnimationName == EnemyControl.Animator.CurrAnim.AnimationName))
            {
                EnemyControl.Animator.EndAnimation(EnemyControl.Animator.CurrAnim.AnimationName);
            }
            EnemyControl.Animator.ChangeAnim(currAnim.AnimationName);
        }

        if (_shootCooldown > 0)
            return;

        Shoot(shootingOrientation);

    }
    void Shoot(Vector2 shootingOrientation)
    {
        Vector2 shootingPosition = (Vector2)_enemy.position + shootingOrientation * _firePointDist;
        Vector2 shootingDirection = ((Vector2)_player.position - shootingPosition).normalized;

        _shootCooldown = 1 / _atkSpeed;

        float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        angle += _proyectileRotationOffset;
        EnemyProyectile proyectile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["EnemyProyectile"], shootingPosition, Quaternion.Euler(0, 0, angle)).GetComponent<EnemyProyectile>();
        proyectile.Initiate(_damage, _proyectileLifetime, _proyectileData);

    }
    public override void KillBehaviour()
    {
        base.KillBehaviour();
        _shootCooldown = 0;
        EnemyControl.Animator.EndAnimation(_currAnim.AnimationName);
    }

}
