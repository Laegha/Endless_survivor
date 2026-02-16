using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursedPlayerEffect : PlayerStatusEffect
{
    new public static int maxStacks => 1;
    [SerializeField] RandomBetweenTwoConstants _timeBetweenMovements;
    [SerializeField] RandomBetweenTwoConstants _movementDuration;

    Vector2 _currMovement;
    float _currWaitingTime;
    float _waitingTimer;
    float _movingTimer;
    bool _isMoving = false;

    public override void Initialize(PlayerStatusEffect original)
    {
        base.Initialize(original);
        var cursedOriginal = original as CursedPlayerEffect;
        _timeBetweenMovements = cursedOriginal._timeBetweenMovements;
        _movementDuration = cursedOriginal._movementDuration;
        
    }

    public override void Update()
    {
        base.Update();
        if (_isMoving)
        {
            SetPlayerMovement();
            _movingTimer -= Time.deltaTime;
            if (_movingTimer <= 0)
                StopMovement();
            return;
        }
        _waitingTimer += Time.deltaTime;
        if (_waitingTimer < _currWaitingTime)
            return;

        SetMovement();
        _movingTimer = _movementDuration.rand;
    }
    void SetMovement()
    {
        _isMoving = true; 
        _waitingTimer = 0;
        _currMovement = Random.insideUnitCircle * PlayerControl.pc.PlayerStats.MinSpeed;
    }
    void StopMovement()
    {
        _isMoving = false;
        _currWaitingTime = _timeBetweenMovements.rand;

        PlayerControl.pc.PlayerRb.velocity = Vector2.zero;
    }

    void SetPlayerMovement()
    {
        PlayerControl.pc.PlayerRb.velocity = _currMovement;
    }
}
