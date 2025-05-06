using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public abstract class AttackGfxInterface
{
    public abstract Type weaponType { get; }
}
