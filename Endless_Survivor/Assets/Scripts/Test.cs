using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] WeaponData firstWeaponData;
    [SerializeField] WeaponData secondWeaponData;
    [SerializeField] Transform player;
    void Start()
    {
        print(Mathf.Log(5, 2));
    }

}
