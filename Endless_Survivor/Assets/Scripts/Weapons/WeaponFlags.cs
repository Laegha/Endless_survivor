using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponFlags
{
    public enum IWeaponTag
    {
        Melee,
        Ranged,
        HighDMG,
        LowDMG,
        HighAtkSpd,
        LowAtkSpd
    }
    [System.Flags]
    public enum IWeaponPool
    {
        None = 0,
        Regular = 1 << 0,
        Sea = 1 << 1,
        Cute = 1 << 2,
        Cutes = 1 << 3

    }
}
