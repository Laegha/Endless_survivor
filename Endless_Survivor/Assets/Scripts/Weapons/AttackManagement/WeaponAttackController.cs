using System;
using UnityEngine;
[Serializable]
public class WeaponAttackController
{
    public static bool isUsable => false;
    [SerializeField] string _attackId;
    [SerializeField] CustomAnimation _attackAnimation;
    [SerializeField] int _attackFrame;
    [SerializeField] AttackEffectsHolder _weaponAttackEffects = new();
    [SerializeField] float _damageMultiplier = 1;
    [SerializeField] DamageInfo.DamageType _damageType;

    WeaponControl _weaponControl;
    WeaponStats _weaponStats;
    Action<Attack> _initializeAttack;

    DamageInfo DamageInfo
    {
        get
        {
            return new DamageInfo(_weaponControl.WeaponAttackManager.WeaponStats.Damage, _damageType);
        }
    }
    public float Damage 
    {
        get
        {
            return DamageInfo.CalculatedDamage * _damageMultiplier;
        }
    }
    public string AttackId {  get { return _attackId; } }
    public string AnimationName { get { return _attackAnimation.AnimationName; } }
    public WeaponStats WeaponStats {  get { return _weaponStats; } set { _weaponStats = value; } }
    public WeaponControl WeaponControl { get { return _weaponControl; } }
    public Action<Attack> InitializeAttack { get { return _initializeAttack; } set { _initializeAttack = value; }  }
    public AttackEffectsHolder WeaponAttackEffects { get { return _weaponAttackEffects; } set { _weaponAttackEffects = value; } }

    public virtual void Initialize(WeaponControl weaponControl, WeaponAttackController original)
    {
        _attackId = original._attackId;
        _weaponControl = weaponControl;
        _weaponStats = weaponControl.WeaponAttackManager.WeaponStats;

        _attackAnimation = new(weaponControl.WeaponAnimator, original._attackAnimation);
        _attackAnimation.Events.Add(new(null, _attackFrame, Attack));
        _attackAnimation.Events.Add(new(null, _attackAnimation.Frames.Length - 1, () => WeaponControl.WeaponAnimator.ChangeAnim("Idle", true)));

        AttackEffectsHolder attackEffectsHolder = new();
        attackEffectsHolder.availableEffects = new(original._weaponAttackEffects.availableEffects);
        _weaponAttackEffects = attackEffectsHolder;

        _damageMultiplier = original._damageMultiplier;
        _damageType = original._damageType;
        _initializeAttack += SetWeaponOnAttack;
    }

    public virtual void Start()
    {
    }

    public virtual void Update()
    {
        
    }

    public virtual void End()
    {

    }

    public virtual void ChangeAttackGfx(AttackGfxInterface gfxInterface) { }

    public virtual void StartAttack()
    {
    }

    public virtual void Attack(Vector2 attackPosOffset, float attackRotationOffset, bool isSecondaryAttack)
    {

    }

    public virtual void Attack()
    {
        PlayerControl.pc.PassiveItemManager.WeaponAttack(WeaponControl.WeaponAttackManager);
        WeaponControl.WeaponAttackManager.FinishAttack();

    }

    void SetWeaponOnAttack(Attack attack)
    {
        attack.ParentWeapon = this;
    }
}
