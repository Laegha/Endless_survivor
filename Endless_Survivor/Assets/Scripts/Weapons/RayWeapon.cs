using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RayWeapon : ShootingWeapon
{
    RayData _rayData;
    public RayData RayData { set {_rayData = value; } }
    public override void Start()
    {
        base.Start();
        InitializeAttack += InitiateLaser;
    }
    public override void Attack()
    {
        base.Attack();

        WeaponControl.WeaponAnimator.ChangeAnim("Attack");
        RayAttack rayAttack = Instantiate(GameManager.gm.prefabHolder.Prefabs["Laser"], Vector2.zero, Quaternion.identity).GetComponent<RayAttack>();

        InitializeAttack(rayAttack);
    }

    void InitiateLaser(Attack attack)
    {
        var rayAttack = attack as RayAttack;
        rayAttack.Attack(WeaponStats.Damage + GameManager.gm.selectedCharacter.PlayerStats.Damage, _rayData, FirePoint);

    }

}
