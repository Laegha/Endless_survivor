using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class WeaponAttackControllerTypeHelper 
{
    public enum AttackControllerTypes
    {
        Proyectile,
        Ray,
        Melee
    }
    public static Type GetWeaponTypeFromEnum(AttackControllerTypes type)
    {
        if(type == AttackControllerTypes.Proyectile)
            return typeof(ProyectileWeaponAttackController);
        if (type == AttackControllerTypes.Ray)
            return typeof(RayWeaponAttackController);
        if (type == AttackControllerTypes.Melee)
            return typeof(MeleeWeaponAttackController);
        return null;
    }
}
