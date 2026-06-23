using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] DamageInfo.DamageType DamageType;
    private void Start()
    {
        DamageInfo.DamageType[] damageTypes = (DamageInfo.DamageType[])Enum.GetValues(typeof(DamageInfo.DamageType));
        foreach (var damageType in damageTypes)
        {
            if (!DamageType.HasFlag(damageType))
                continue;
            Debug.Log(damageType.ToString());

        }

    }
}