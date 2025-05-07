using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProyectileAttackGfxInterface : AttackGfxInterface
{
    public Sprite proyectileSprite;
    public Material proyectileMaterial;
    public override Type weaponType => typeof(ProyectileWeapon);
}
