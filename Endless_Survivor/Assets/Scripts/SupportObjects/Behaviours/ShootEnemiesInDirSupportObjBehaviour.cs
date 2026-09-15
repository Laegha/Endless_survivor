using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootEnemiesInDirSupportObjBehaviour : SupportObjectBehaviour
{
    new public static int maxStacks => -1;
    [Tooltip("The offset in pixels (with a 32 x 32 resolution) from the center of the sprite where the proyectile will start")][SerializeField] Vector2 _firePoint;
    [SerializeField] float _shootAngle;
    [SerializeField] WeaponStats _attackStats;
    [SerializeField] WeaponStats _statsScaling;
    [SerializeField] ProyectileData _proyectileData;
    [SerializeField] CustomAnimation _shootAnim;
    [SerializeField] int _shootFrame;

    float _cooldownTimer;
    float _attackCooldown => 1 / _attackStats.AttackSpeed;
    Vector2 _firePointInUnits;
    public override void Initiate(SupportObjectControl control, SupportObjectBehaviour original)
    {
        base.Initiate(control, original);
        var shootEnemiesOriginal = original as ShootEnemiesInDirSupportObjBehaviour;
        _firePoint = shootEnemiesOriginal._firePoint;
        _firePointInUnits = _firePoint / 32;
        _shootAngle = shootEnemiesOriginal._shootAngle;
        _attackStats = new(shootEnemiesOriginal._attackStats);
        _statsScaling = new(shootEnemiesOriginal._statsScaling);
        _attackStats.SetTrueLevelStats(_statsScaling, IntensityManager.im.CurrIntensityLevel);
        _proyectileData = new(shootEnemiesOriginal._proyectileData);
        _shootAnim = new(ObjControl.Animator, shootEnemiesOriginal._shootAnim);
        _shootFrame = shootEnemiesOriginal._shootFrame;
        _shootAnim.Events.Add(new(null, _shootFrame, CreateProyectile));
        _shootAnim.Events.Add(new(null, _shootAnim.Frames.Length -1, EndAnim));
        ObjControl.Animator.AddAnimations(new() { _shootAnim });
        OnUpdate += DecreaseCooldownTimer;
        OnUpdate += CheckEnemy;
    }

    void CheckEnemy()
    {
        if (_cooldownTimer > 0)
            return;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)ObjControl.transform.position + _firePointInUnits, Utility.GetPointInCircle(1, _shootAngle), _attackStats.Range);
        //Debug.Log(hit.collider?.transform.root.name);
        Debug.Log("RAY POS " + ((Vector2)ObjControl.transform.position + _firePointInUnits));
        Debug.Log("RAY DIR " + (Utility.GetPointInCircle(1, _shootAngle)));
        if (!hit || Utility.FindFirstComponentInParent<EnemyControl>(hit.collider.gameObject) == null)
            return;
        _cooldownTimer = _attackCooldown;
        ObjControl.Animator.ChangeAnim(_shootAnim.AnimationName);
    }
    void DecreaseCooldownTimer()
    {
        if (_cooldownTimer <= 0)
            return;
        _cooldownTimer -= Time.deltaTime;
    }
    void CreateProyectile()
    {
        ProyectileAttack proyectile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Proyectile"]).GetComponent<ProyectileAttack>();
        Vector2 proyectilePos = (Vector2)ObjControl.transform.position + _firePointInUnits;
        InitiateProyectile(proyectile, proyectilePos, Quaternion.Euler(0, 0, _shootAngle - 90));
    }
    void InitiateProyectile(ProyectileAttack proyectile, Vector2 proyectilePosition, Quaternion proyectileRotation, List<Collider2D> ignoreColliders = null)
    {
        proyectile.StartProyectile(proyectilePosition, proyectileRotation);

        float proyectileSpeed = _proyectileData.ProyectileSpeed;
        float proyectileLifeTime = _attackStats.Range;
        int proyectileDamage = (int)_attackStats.Damage;
        proyectile.Initiate(proyectileDamage, _attackStats.Knockback, proyectileSpeed, proyectileLifeTime, _proyectileData, _proyectileData.ProyectileSpread, ignoreColliders);
    }
    void EndAnim()
    {
        ObjControl.Animator.EndAnimation(_shootAnim.AnimationName);
        ObjControl.Animator.ChangeAnim(ObjControl.BehaviourManager.SupportObjData.IdleAnimation.AnimationName);

    }
}
