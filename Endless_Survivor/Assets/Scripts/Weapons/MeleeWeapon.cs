using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeWeapon : Weapon
{
    MeleeData _meleeData;
    public MeleeData MeleeData { set { _meleeData = value; } }
    public override void Start()
    {
        base.Start();
        InitializeAttack += InitiateMelee;
    }
    public override void Attack()
    {
        base.Attack();
        WeaponControl.WeaponAnimator.ChangeAnim("Attack");
        MeleeAttack meleeAttack = Instantiate(GameManager.gm.prefabHolder.Prefabs["Melee"], transform.position, transform.rotation).GetComponent<MeleeAttack>();
        meleeAttack.transform.SetParent(transform, true);
        

        InitializeAttack(meleeAttack);
    }
    void InitiateMelee(Attack attack)
    {
        var meleeAttack = attack as MeleeAttack;
        meleeAttack.Attack(WeaponStats.Damage, _meleeData);
    }
}
