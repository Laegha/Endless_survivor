using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class ShootPlayerEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;

    [SerializeField] int _damage = 1;
    [SerializeField] float _proyectileLifetime = 3;
    [SerializeField] float _firePointDist = 1.5f;
    [SerializeField] float _proyectileRotationOffset;

    [SerializeField] ProyectileData _proyectileData;
    [SerializeField] LineXConfig _lineConfig;

    [SerializeField] SFXInfo _onShootSFX;

    Transform _player;
    Transform _enemy;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        ShootPlayerEnemyBehaviour originalShootPlayer = original as ShootPlayerEnemyBehaviour;

        _damage = originalShootPlayer._damage;
        _proyectileLifetime = originalShootPlayer._proyectileLifetime;
        _firePointDist = originalShootPlayer._firePointDist;
        _proyectileRotationOffset = originalShootPlayer._proyectileRotationOffset;


        _proyectileData = originalShootPlayer._proyectileData;
        _lineConfig = originalShootPlayer._lineConfig;

        _onShootSFX = originalShootPlayer._onShootSFX;

        _player = PlayerControl.pc.transform;
        _enemy = enemyControl.transform;

    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        Vector2 distance = _player.position - EnemyControl.transform.position;
        Vector2 shootingOrientation = distance.normalized;

        Shoot(shootingOrientation);
        KillBehaviour();

    }
    void Shoot(Vector2 shootingOrientation)
    {
        Vector2 shootingPosition = (Vector2)_enemy.position + shootingOrientation * _firePointDist;
        Vector2 shootingDirection = ((Vector2)_player.position - shootingPosition).normalized;

        float angle = Mathf.Atan2(shootingDirection.y, shootingDirection.x) * Mathf.Rad2Deg;
        angle += _proyectileRotationOffset;
        EnemyProyectile proyectile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["EnemyProyectile"], shootingPosition, Quaternion.Euler(0, 0, angle)).GetComponent<EnemyProyectile>();
        proyectile.Initiate(_damage, _proyectileLifetime, _proyectileData);
        
        float proyectileMaxDist = _proyectileData.ProyectileSpeed * _proyectileLifetime;
        Vector2 proyectileDir = proyectile.transform.right;
        var distRay = Physics2D.Raycast(shootingPosition, proyectileDir, proyectileMaxDist, LayerMask.GetMask("Map"));
        float proyectileDist = distRay ? (shootingPosition - distRay.point).magnitude : proyectileMaxDist;
        LineXConfig lineConfig = new(_lineConfig, shootingPosition, proyectileDir, proyectileDist, () => proyectile == null);
        LineXManager.lm.DrawLine(lineConfig);

        SoundFXManager.sm.PlaySfx(_onShootSFX, EnemyControl.transform.position);

    }

}
