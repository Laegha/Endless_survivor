using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameUIManager : MonoBehaviour
{
    [SerializeField] GameObject _menuBG;
    [SerializeField] WeaponPickupMenu _weaponPickupMenu;
    [SerializeField] WeaponSwitchMenu _weaponOverrideMenu;
    [SerializeField] PassiveItemPickupMenu _passiveItemPickupMenu;
    [SerializeField] PlayerHPBar _playerHPBar;
    [SerializeField] IntensityUI _intensityUI;
    [SerializeField] UIPointerManager _pointerManager;
    public static GameUIManager instance;
    public static GameUIManager uiManager {  get { return instance; }}
    public WeaponPickupMenu WeaponPickupMenu { get { return _weaponPickupMenu; }}
    public WeaponSwitchMenu WeaponOverrideMenu { get { return _weaponOverrideMenu; }}
    public PassiveItemPickupMenu PassiveItemPickupMenu { get { return _passiveItemPickupMenu; }}
    public PlayerHPBar PlayerHPBar { get { return _playerHPBar; }}
    public IntensityUI IntensityUI { get { return _intensityUI; }}
    public UIPointerManager PointerManager { get { return _pointerManager; }}
    void Awake()
    {
        instance = this;
    }
    public void MenuDisplayed()
    {
        _menuBG.SetActive(true);
        Time.timeScale = 0;
    }
    public void MenuHid()
    {
        _menuBG.SetActive(false);
        Time.timeScale = 1;
    }
}
