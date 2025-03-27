using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] WeaponPickupMenu _weaponPickupMenu;
    [SerializeField] WeaponOverrideMenu _weaponOverrideMenu;
    public static GameUIManager instance;
    public static GameUIManager uiManager {  get { return instance; }}
    public WeaponPickupMenu WeaponPickupMenu { get { return _weaponPickupMenu; }}
    public WeaponOverrideMenu WeaponOverrideMenu { get { return _weaponOverrideMenu; }}
    void Awake()
    {
        instance = this;
    }
}
