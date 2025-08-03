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
    SpriteMaterialFlashing _inmutiyFlashing;
    SFXInfo _onHitSound;
    SFXInfo _onDeathSound;
    public SFXInfo OnHitSound { set { _onHitSound = value; } }
    public SFXInfo OnDeathSound { set { _onDeathSound = value; } }

    public void Start()
    {
        InitializeHP(_playerControl.PlayerStats.MaxHealth);
        GameUIManager.uiManager.PlayerHPBar.SetHP(RemainingHP, MaxHP);
        _inmutiyFlashing = new SpriteMaterialFlashing(_playerControl.PlayerMaterialManager, _inmunityFlashingTime,new MaterialOverride(_flashingAuthority, _inmunityFlashingMaterial));
    }

    private void Update()
    {
        if (!_isInmune)
            return;
        _inmunityTimer -= Time.deltaTime;
        _inmutiyFlashing.Update();
        if (_inmunityTimer <= 0)
        {
            _inmutiyFlashing.End();
            _isInmune = false;
        }
        
    }

    public override void TakeDamage(int incomingDamage)
    {
        if (_isInmune) 
            return;
        SoundFXManager.sm.PlaySfx(_onHitSound, transform.position);
        _inmutiyFlashing.Start();
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
