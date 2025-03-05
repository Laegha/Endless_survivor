using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingWeapon : Weapon
{
    Transform _firePoint;
    public Transform FirePoint { get { return _firePoint; } set { _firePoint = value; } }
}
