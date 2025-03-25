using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] WeaponPickupMenu _weaponPickupMenu;
    public static GameUIManager instance;
    public static GameUIManager uiManager {  get { return instance; }}
    public WeaponPickupMenu WeaponPickup { get { return _weaponPickupMenu; }}
    void Awake()
    {
        instance = this;
    }
}
