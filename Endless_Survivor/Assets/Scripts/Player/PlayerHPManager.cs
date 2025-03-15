using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHPManager : MonoBehaviour
{
    int _remainingHP;
    bool _isInmune = false;
    float _inmunityTimer = .5f;
    PlayerControl _playerControl;
    public PlayerControl PlayerControl {  get { return _playerControl; } }

    public void Initialize(PlayerControl playerControl)
    {
        _playerControl = playerControl;
        _remainingHP = _playerControl.PlayerStats.MaxHealth;
    }

    private void Update()
    {
        if (!_isInmune)
            return;

        _inmunityTimer -= Time.deltaTime;
        if (_inmunityTimer <= 0)
            _isInmune = false;

    }

    public void TakeDamage(int incomingDamage)
    {
        if (_isInmune) 
            return;

        _remainingHP -= incomingDamage;
        _isInmune = true;
        if (_remainingHP <= 0)
        {
            Die();
            return;
        }

    }

    void Die()
    {

    }
}
