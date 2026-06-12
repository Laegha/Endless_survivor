using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
[Serializable]
public class ProyectileWeaponAttackController : ShootingWeaponAttackController
{
    new public static bool isUsable => true;
    [SerializeField] ProyectileData _proyectileData;
    public override void Initialize(WeaponControl weaponControl, WeaponAttackController original)
    {
        base.Initialize(weaponControl, original);
        ProyectileWeaponAttackController proyectileOriginal = original as ProyectileWeaponAttackController;
        _proyectileData = new(proyectileOriginal._proyectileData);
    }
    public override void StartAttack()
    {
        base.StartAttack();
        WeaponControl.WeaponAnimator.ChangeAnim(AnimationName, false, true);
    }
    public override void Attack()
    {
        base.Attack();

        ProyectileAttack proyectile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Proyectile"]).GetComponent<ProyectileAttack>();
        InitializeAttack?.Invoke(proyectile);
        Vector2 proyectilePos = ShootingPosition + (_proyectileData.ColliderSize.x / 2) * (Vector2)FirePoint.right.normalized;
        InitiateProyectile(proyectile, proyectilePos, FirePoint.rotation);
    }
    public override void Attack(Vector2 attackPosOffset, float attackRotationOffset, bool isSecondaryAttack)
    {
        base.Attack(attackPosOffset, attackRotationOffset, isSecondaryAttack);

        ProyectileAttack proyectile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Proyectile"]).GetComponent<ProyectileAttack>();
        InitializeAttack?.Invoke(proyectile);
        var firepointRotation = FirePoint.eulerAngles.z * Mathf.Deg2Rad;
        //Vector2 offsetYDirection = ; // (p1, p2) * (o1, o2) = p1*o1 + p2*o2 = 0 => o2p2 = -o1p1 => p2 = -o1/o2p1
        var pixelOffset = attackPosOffset / 32;
        Vector2 offsetXDirection = -new Vector2(Mathf.Cos(firepointRotation), Mathf.Sin(firepointRotation)).normalized;//it's inverted because the firepoint faces left
        Vector2 offsetYDirection = new Vector2(1, (-offsetXDirection.x/offsetXDirection.y) * 1).normalized;
        Vector2 offsetXMovement = offsetXDirection * pixelOffset.x;
        Vector2 offsetYMovement = offsetYDirection * pixelOffset.y;

        Vector2 offsetMovement = offsetXMovement + offsetYMovement;
        
        proyectile.IsSecondaryAttack = isSecondaryAttack;

        Vector2 originalProyectilePos = ShootingPosition + (_proyectileData.ColliderSize.x / 2) * (Vector2)FirePoint.right.normalized;
        InitiateProyectile(proyectile, originalProyectilePos + (offsetXDirection.x * offsetXDirection.y > 0 ? offsetMovement : -offsetMovement), FirePoint.rotation);// I only tested this with y movement, idk with x

    }
    public override void Attack(Vector2 attackPos, Vector2 attackDirection, bool isSecondaryAttack, out Attack createdAttack, List<Collider2D> ignoreColliders = null)
    {
        base.Attack(attackPos, attackDirection, isSecondaryAttack, out createdAttack, ignoreColliders);
        ProyectileAttack proyectile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Proyectile"]).GetComponent<ProyectileAttack>();
        createdAttack = proyectile;
        proyectile.IsSecondaryAttack = isSecondaryAttack;
        InitializeAttack?.Invoke(proyectile);
        float proyectileRotation = Mathf.Atan2(attackDirection.y, attackDirection.x) * Mathf.Rad2Deg;
        InitiateProyectile(proyectile, attackPos, Quaternion.Euler(0,0,proyectileRotation), ignoreColliders);
    }
    void InitiateProyectile(ProyectileAttack proyectile, Vector2 proyectilePosition, Quaternion proyectileRotation, List<Collider2D> ignoreColliders = null)
    {
        proyectile.transform.position = proyectilePosition;
        proyectile.transform.rotation = proyectileRotation;

        float proyectileSpeed = _proyectileData.ProyectileSpeed;
        float proyectileLifeTime = WeaponStats.Range;
        int proyectileDamage = (int)Damage;
        proyectile.Initiate(proyectileDamage, WeaponStats.Knockback, proyectileSpeed, proyectileLifeTime, _proyectileData, _proyectileData.ProyectileSpread, ignoreColliders);
    }
}
