using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPManager : HP
{
    bool _isInmune = false;
    float _inmunityTime = .5f;
    float _inmunityTimer = 0;
    [SerializeField] PlayerControl _playerControl;
    [SerializeField]float _inmunityFlashingTime = .1f;
    [SerializeField] Material _inmunityFlashingMaterial;
    static readonly int _flashingAuthority;
    SpriteMaterialFlashing _inmunityFlashing;
    SFXInfo _onHitSound;
    SFXInfo _onDeathSound;
    public SFXInfo OnHitSound { set { _onHitSound = value; } }
    public SFXInfo OnDeathSound { set { _onDeathSound = value; } }

    public void Start()
    {
        InitializeHP(_playerControl.PlayerStats.InitialHP);
        GameUIManager.uiManager.PlayerHPBar.SetHP(RemainingHP, MaxHP);
        _inmunityFlashing = new SpriteMaterialFlashing(_playerControl.PlayerMaterialManager, _inmunityFlashingTime,new MaterialOverride(_flashingAuthority, _inmunityFlashingMaterial));
        WaveManager.wm.OnWaveStarted += IncreaseMaxHP;
    }

    private void Update()
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
    void IncreaseMaxHP()
    {
        var previousMaxHP = MaxHP;
        var remainingHPPercentage = MaxHP / RemainingHP;
        MaxHP = ScalingFunctions.PlayerHPIncrease(ScalingFunctions.CurrWaveLevel-1, PlayerControl.pc.PlayerStats.HPIncrement, PlayerControl.pc.PlayerStats.InitialHP);//the level is -1 so it starts at 0 instead of 1
        if (MaxHP == previousMaxHP)
            return;
        RemainingHP = MaxHP / remainingHPPercentage;
        GameUIManager.uiManager.PlayerHPBar.SetHP(RemainingHP, MaxHP);
        //display particles with MaxHP - previousMaxHP

    }

    public override void TakeDamage(int incomingDamage)
    {
        if (_isInmune) 
            return;
        SoundFXManager.sm.PlaySfx(_onHitSound, transform.position);
        _inmunityFlashing.Start();
        _isInmune = true;
        _inmunityTimer = _inmunityTime;
        base.TakeDamage(incomingDamage);
        GameUIManager.uiManager.PlayerHPBar.SetHP(RemainingHP, MaxHP);

    }

    public override void Die()
    {
        SoundFXManager.sm.PlaySfx(_onDeathSound, transform.position);
        SceneLoadingFunctions.slf.GameOver();
    }
}
