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
    CharacterData _characterData;
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
    public CharacterData CharacterData { get { return _characterData; } set { _characterData = value; } }
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
    public void ChangeCollider(CapsuleDirection2D newDirection, Vector2 newSize, Vector2 newOffset)
    {
        CapsuleCollider2D collider = GetComponent<CapsuleCollider2D>();
        collider.direction = newDirection;
        collider.size = newSize;
        foreach (Transform child in transform.GetComponentInChildren<Transform>())
        {
            if (child == collider.transform || child == transform)
                continue;
            child.localPosition -= new Vector3(newOffset.x, newOffset.y, 0);
        }
    }
}
