using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ShootInFixedPatternSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] WeaponStats _attackStats;
    [SerializeField] WeaponStats _statsScaling;
    [SerializeField] CustomAnimation _shootAnim;
    [SerializeReference] IPattern _shootPattern;
    [SerializeField] RandomBetweenTwoConstants _proyectileCount;
    [SerializeField] ProyectileData _proyectileData;

    float _shootCooldown => 1 / _attackStats.AttackSpeed;
    float _shootTimer;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var shootInPatternOriginal = original as ShootInFixedPatternSupportObjBehaviour;
        _attackStats = shootInPatternOriginal._attackStats;
        _statsScaling = shootInPatternOriginal._statsScaling;
        _shootAnim = new(ObjControl.Animator, shootInPatternOriginal._shootAnim);
        ObjControl.Animator.AddAnimations(new() { _shootAnim });
        _shootPattern = shootInPatternOriginal._shootPattern;
        _proyectileCount = shootInPatternOriginal._proyectileCount;
        _proyectileData = new(shootInPatternOriginal._proyectileData);

        OnUpdate += ReduceCooldown;
    }

    void ReduceCooldown()
    {
        _shootTimer += Time.deltaTime;
        if (_shootTimer < _shootCooldown)
            return;
        Shoot();
    }
    void Shoot()
    {
        int proyectileCount = (int)_proyectileCount.rand;
        Vector2[] proyectilePositions = _shootPattern.GetPositions(ObjControl.transform.position, proyectileCount).ToArray();
        foreach (var position in proyectilePositions)
        {
            Vector2 direction = position - (Vector2)ObjControl.transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            CreateProyectile(position, angle);
        }
    }
    void CreateProyectile(Vector2 position, float angle)
    {
        ProyectileAttack proyectile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Proyectile"]).GetComponent<ProyectileAttack>();
        InitiateProyectile(proyectile, position, Quaternion.Euler(0, 0, angle));
    }
    void InitiateProyectile(ProyectileAttack proyectile, Vector2 proyectilePosition, Quaternion proyectileRotation, List<Collider2D> ignoreColliders = null)
    {
        proyectile.StartProyectile(proyectilePosition, proyectileRotation);

        float proyectileSpeed = _proyectileData.ProyectileSpeed;
        float proyectileLifeTime = _attackStats.Range;
        int proyectileDamage = (int)_attackStats.Damage;
        proyectile.Initiate(proyectileDamage, _attackStats.Knockback, proyectileSpeed, proyectileLifeTime, _proyectileData, _proyectileData.ProyectileSpread, ignoreColliders);
    }
}
