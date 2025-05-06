using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ProyectileAttackGfxInterface : AttackGfxInterface
{
    [SerializeField] Sprite _proyectileSprite;
    [SerializeField] Material _proyectileMaterial;
    public override Type weaponType => typeof(ProyectileWeapon);
}
