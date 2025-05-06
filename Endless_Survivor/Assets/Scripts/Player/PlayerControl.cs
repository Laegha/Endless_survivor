using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    static PlayerControl instance;
    public static PlayerControl pc {  get { return instance; } }
    private void Awake()
    {
        instance = this;
    }
    [SerializeField] PlayerAnimator _playerAnimator;
    PlayerStats _playerStats;
    [SerializeField] Rigidbody2D _playerRb;
    [SerializeField] PlayerHPManager _playerHPManager;
    [SerializeField] PlayerWeaponManager _playerWeaponManager;
    [SerializeField] PassiveItemManager _passiveItemManager;
    [SerializeField] SpriteRenderer[] _renderers;
    public PlayerAnimator PlayerAnimator { get { return _playerAnimator; } }
    public PlayerStats PlayerStats { get { return _playerStats; } set { _playerStats = value; } }
    public Rigidbody2D PlayerRb { get { return _playerRb; } }
    public PlayerHPManager PlayerHPManager { get { return _playerHPManager; } }
    public PassiveItemManager PassiveItemManager {  get { return _passiveItemManager; } }
    public PlayerWeaponManager WeaponManager { get { return _playerWeaponManager; } }
    public SpriteRenderer[] Renderers { get { return _renderers; } }

    private void Start()
    {
        _playerStats = new PlayerStats(GameManager.gm.selectedCharacter.PlayerStats);
    }
}
