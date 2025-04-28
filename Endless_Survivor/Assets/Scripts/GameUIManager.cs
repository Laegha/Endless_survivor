using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] WeaponPickupMenu _weaponPickupMenu;
    [SerializeField] WeaponSwitchMenu _weaponOverrideMenu;
    [SerializeField] PlayerHPBar _playerHPBar;
    public static GameUIManager instance;
    public static GameUIManager uiManager {  get { return instance; }}
    public WeaponPickupMenu WeaponPickupMenu { get { return _weaponPickupMenu; }}
    public WeaponSwitchMenu WeaponOverrideMenu { get { return _weaponOverrideMenu; }}
    public PlayerHPBar PlayerHPBar { get { return _playerHPBar; }}
    void Awake()
    {
        instance = this;
    }
}
