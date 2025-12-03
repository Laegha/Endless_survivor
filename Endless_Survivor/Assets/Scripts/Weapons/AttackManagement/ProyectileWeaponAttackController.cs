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

    public override void StartAttack()
    {
        base.StartAttack();
        WeaponControl.WeaponAnimator.ChangeAnim("Attack");
    }
    public override void Attack()
    {
        base.Attack();

        ProyectileAttack proyectile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Proyectile"]).GetComponent<ProyectileAttack>();
        InitializeAttack?.Invoke(proyectile);
        InitiateProyectile(proyectile, FirePoint.position, FirePoint.rotation);
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

        InitiateProyectile(proyectile,(Vector2)FirePoint.position + (offsetXDirection.x * offsetXDirection.y > 0 ? offsetMovement : -offsetMovement), FirePoint.rotation);// I only tested this with y movement, idk with x

    }
    void InitiateProyectile(ProyectileAttack proyectile, Vector2 proyectilePosition, Quaternion proyectileRotation)
    {
        proyectile.transform.position = proyectilePosition;
        proyectile.transform.rotation = proyectileRotation;

        float proyectileSpeed = _proyectileData.ProyectileSpeed;
        float proyectileLifeTime = WeaponStats.Range;
        int proyectileDamage = (int)Damage;
        proyectile.Initiate(proyectileDamage, WeaponStats.Knockback, proyectileSpeed, proyectileLifeTime, _proyectileData, _proyectileData.ProyectileSpread);
    }
}
