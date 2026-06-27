using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPManager : HP
{
    [SerializeField] PlayerControl _playerControl;
    [SerializeField]float _inmunityFlashingTime = .1f;
    [SerializeField] Material _inmunityFlashingMaterial;
    static readonly int _flashingAuthority;
    SpriteMaterialFlashing _inmunityFlashing;
    SFXInfo _onHitSound;
    SFXInfo _onDeathSound;

    bool _isInmune = false;
    float _inmunityTime = .5f;
    float _inmunityTimer = 0;

    float _regenerationTimer;

    Action _onUpdateActions;
    public bool IsInmune {  get { return _isInmune; } }
    public SFXInfo OnHitSound { set { _onHitSound = value; } }
    public SFXInfo OnDeathSound { set { _onDeathSound = value; } }

    public void Start()
    {
        InitializeHP(_playerControl.PlayerStats.InitialHP);
        GameUIManager.uiManager.PlayerHPBar.SetHP(RemainingHP, MaxHP);
        _inmunityFlashing = new SpriteMaterialFlashing(_playerControl.PlayerMaterialManager, _inmunityFlashingTime,new MaterialOverride(_flashingAuthority, _inmunityFlashingMaterial));
        IntensityManager.im.OnLevelIncrease += IncreaseMaxHP;
        _onUpdateActions += HandleInmunity;
        _onUpdateActions += HandleRegeneration;

    }

    private void Update()
    {
        _onUpdateActions?.Invoke();
        
    }
    void HandleInmunity()
    {
        if (!_isInmune)
            return;
        _inmunityTimer -= Time.deltaTime;
        _inmunityFlashing.Update();
        if (_inmunityTimer <= 0)
        {
            _inmunityFlashing.End();
            _isInmune = false;
        }
    }
    void HandleRegeneration()
    {
        _regenerationTimer -= Time.deltaTime;
        if(_regenerationTimer <= 0)
        {
            //Heal 1 hp
            Heal(1);    
            _regenerationTimer = 1 / PlayerControl.pc.PlayerStats.HPRegeneration;
        }
    }
    void IncreaseMaxHP()
    {
        var previousMaxHP = MaxHP;
        var remainingHPPercentage = MaxHP / RemainingHP;
        MaxHP = ScalingFunctions.PlayerHPIncrease(ScalingFunctions.CurrScalingLevel-1, PlayerControl.pc.PlayerStats.HPIncrement, PlayerControl.pc.PlayerStats.InitialHP);//the level is -1 so it starts at 0 instead of 1
        if (MaxHP == previousMaxHP)
            return;
        RemainingHP = MaxHP / remainingHPPercentage;
        GameUIManager.uiManager.PlayerHPBar.SetHP(RemainingHP, MaxHP);
        //display particles with MaxHP - previousMaxHP

    }
    public override void Heal(int healedHP)
    {
        base.Heal(healedHP);
        GameUIManager.uiManager.PlayerHPBar.SetHP(RemainingHP, MaxHP);
    }

    public override void TakeDamage(int incomingDamage)
    {
        if (_isInmune) 
            return;
        SoundFXManager.sm.PlaySfx(_onHitSound, transform.position);
        _inmunityFlashing.Start();
        _isInmune = true;
        _inmunityTimer = _inmunityTime;
        base.TakeDamage(ScalingFunctions.PlayerDamageFormula(incomingDamage));
        GameUIManager.uiManager.PlayerHPBar.SetHP(RemainingHP, MaxHP);

    }

    public override void Die()
    {
        SoundFXManager.sm.PlaySfx(_onDeathSound, transform.position);
        GameProgressionManager.gpm.EndRun(PlayerControl.pc.CharacterData.DeathAnimation, PlayerControl.pc.CharacterData.DeathParticles, GameProgressionManager.RunEndType.Lose);
    }
}
