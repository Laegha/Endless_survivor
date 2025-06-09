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
    }
    public override void Attack()
    {
        base.Attack();

        WeaponControl.WeaponAnimator.ChangeAnim("Attack");
        RayAttack laserAttack = Instantiate(GameManager.gm.prefabHolder.Prefabs["Laser"], Vector2.zero, Quaternion.identity).GetComponent<RayAttack>();

        laserAttack.Attack(_rayData.RayExitSpeed, WeaponStats.Damage + GameManager.gm.selectedCharacter.PlayerStats.Damage, _rayData, FirePoint);
    }

}
