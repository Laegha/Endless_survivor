using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[Serializable]
public class RayAttackGfxInterface : AttackGfxInterface
{
    public override Type weaponType => typeof(RayWeapon);
    public Material rayMaterial;
    public float rayWidth;
}
