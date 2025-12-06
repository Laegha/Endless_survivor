using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class RayWeaponAttackController : ShootingWeaponAttackController
{
    new public static bool isUsable => true;
    [SerializeField] RayData _rayData;
    public override void Initialize(WeaponControl weaponControl, WeaponAttackController original)
    {
        base.Initialize(weaponControl, original);
        RayWeaponAttackController rayWeaponOriginal = original as RayWeaponAttackController;
        _rayData = rayWeaponOriginal._rayData;
    }
    public override void StartAttack()
    {
        base.StartAttack();

        WeaponControl.WeaponAnimator.ChangeAnim(AnimationName, false, true);
    }

    public override void Attack()
    {
        base.Attack();

        RayAttack rayAttack = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Laser"], Vector2.zero, Quaternion.identity).GetComponent<RayAttack>();

        InitializeAttack(rayAttack);
        InitiateLaser(rayAttack);
    }
    public override void Attack(Vector2 attackPos, Vector2 attackDirection, bool isSecondaryAttack, List<Collider2D> ignoreColliders)
    {
        base.Attack(attackPos, attackDirection, isSecondaryAttack);
        RayAttack rayAttack = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Laser"], Vector2.zero, Quaternion.identity).GetComponent<RayAttack>();
        rayAttack.IsSecondaryAttack = isSecondaryAttack;
        InitializeAttack(rayAttack);
        rayAttack.Attack((int)Damage, WeaponStats.Knockback, _rayData, attackPos, attackDirection, ignoreColliders);

    }

    void InitiateLaser(Attack attack)
    {
        var rayAttack = attack as RayAttack;
        Vector2 rayDir = FirePoint.right;
        rayAttack.Attack((int)Damage, WeaponStats.Knockback, _rayData, FirePoint.position, rayDir);

    }

    public override void ChangeAttackGfx(AttackGfxInterface gfxInterface)
    {
        base.ChangeAttackGfx(gfxInterface);
    }

}
