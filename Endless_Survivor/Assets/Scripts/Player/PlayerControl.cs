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
    [SerializeField] CustomAnimator _playerAnimator;
    PlayerStats _playerStats;
    AttackEffectsHolder _attackEffectsHolder = new();
    [SerializeField] Rigidbody2D _playerRb;
    [SerializeField] PlayerHPManager _playerHPManager;
    [SerializeField] PlayerWeaponManager _playerWeaponManager;
    [SerializeField] PassiveItemManager _passiveItemManager;
    [SerializeField] PlayerStatusEffectManager _statusEffectManager;
    [SerializeField] MaterialManager _playerMaterialManager;
    [SerializeField] SpriteRenderer _mainRenderer;
    [SerializeField] PlayerCostumeManager _costumeManager;
    public CustomAnimator PlayerAnimator { get { return _playerAnimator; } }
    public PlayerStats PlayerStats { get { return _playerStats; } set { _playerStats = value; } }
    public AttackEffectsHolder EffectsHolder {  get { return _attackEffectsHolder; } }
    public Rigidbody2D PlayerRb { get { return _playerRb; } }
    public PlayerHPManager PlayerHPManager { get { return _playerHPManager; } }
    public PassiveItemManager PassiveItemManager {  get { return _passiveItemManager; } }
    public PlayerStatusEffectManager StatusEffectManager {  get { return _statusEffectManager; } }
    public PlayerWeaponManager WeaponManager { get { return _playerWeaponManager; } }
    public MaterialManager PlayerMaterialManager { get { return _playerMaterialManager; } }
    public SpriteRenderer MainRenderer { get { return _mainRenderer; } }
    public PlayerCostumeManager CostumeManager {  get { return _costumeManager; } }

    private void Start()
    {
        WeaponAim.SharedAttackTargets.Clear();
    }
    private void Update()
    {
        WeaponAim.SharedAttackTargets.RemoveAll(x => x.obj == null);
    }
}
