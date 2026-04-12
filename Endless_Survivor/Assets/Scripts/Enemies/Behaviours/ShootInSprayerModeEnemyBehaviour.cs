using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class ShootInSprayerModeEnemyBehaviour : EnemyBehaviour
{
    new public static int maxStacks => -1;
    [SerializeField] int _damage = 1;
    [SerializeField] float _proyectileLifetime = 3;
    [SerializeField] float _firePointDist;
    [SerializeField] ProyectileData _proyectileData;
    [SerializeField] List<PlayerStatusEffectData> _appliedPlayerStatusOnHit;

    [SerializeField] bool _flipVerticallyIfPlayerIsLeft;
    [SerializeField] bool _flipHorizontallyIfPlayerIsDown;
    [SerializeField] float _startAngle;   
    [SerializeField] AnimationCurve _angleChangeCycle;// it would be AWESOME if you could put key rotations with a given value between 0 and 1 that represents the moment in the cycle and then, if the current time is between keys, interpolate
    [SerializeField] float _cycleHeightMultiplier = 1;
    [Range(0, 1)][SerializeField] float _cycleMinSpeed;
    [Range(0, 1)][SerializeField] float _cycleMaxSpeed;
    [SerializeField] bool _resetCycleIfNotShootForTime;

    const int _curveLength = 1;
    float _currCycleTime;
    Vector2 _playerDirectionOnCurrCycle;

    const float _bufferTime = .5f;
    float _bufferTimer;

    public override void Initialize(EnemyBehaviour original, EnemyControl enemyControl)
    {
        base.Initialize(original, enemyControl);
        var shootInSprayOriginal = original as ShootInSprayerModeEnemyBehaviour;
        _damage = shootInSprayOriginal._damage;
        _proyectileLifetime = shootInSprayOriginal._proyectileLifetime;
        _firePointDist = shootInSprayOriginal._firePointDist;
        _proyectileData = shootInSprayOriginal._proyectileData;
        _appliedPlayerStatusOnHit = new(shootInSprayOriginal._appliedPlayerStatusOnHit);
        _flipVerticallyIfPlayerIsLeft = shootInSprayOriginal._flipVerticallyIfPlayerIsLeft;
        _flipHorizontallyIfPlayerIsDown = shootInSprayOriginal._flipHorizontallyIfPlayerIsDown;
        _startAngle = shootInSprayOriginal._startAngle;
        _angleChangeCycle = shootInSprayOriginal._angleChangeCycle;
        _cycleHeightMultiplier = shootInSprayOriginal._cycleHeightMultiplier;
        _cycleMinSpeed = Mathf.Clamp(shootInSprayOriginal._cycleMinSpeed, 0, shootInSprayOriginal._cycleMaxSpeed);
        _cycleMaxSpeed = Mathf.Clamp(shootInSprayOriginal._cycleMaxSpeed, shootInSprayOriginal._cycleMinSpeed, Mathf.Infinity);
	_resetCycleIfNotShootForTime = shootInSprayOriginal._resetCycleIfNotShootForTime;
    }

    public override void PassiveUpdate()
    {
        base.PassiveUpdate();
        if (_currCycleTime == 0 || !_resetCycleIfNotShootForTime)
            return;
        _bufferTimer += Time.deltaTime;
        if (_bufferTimer >= _bufferTime)
            _currCycleTime = 0;
    }
    public override void ActiveUpdate()
    {
        base.ActiveUpdate();
        _bufferTimer = 0;
        if(_currCycleTime == 0)
        {
            //calculate player direction
            _playerDirectionOnCurrCycle = PlayerControl.pc.transform.position - EnemyControl.transform.position;
        }
        //Create bullet in current rotation
        float bulletAngle = _startAngle + _angleChangeCycle.Evaluate(_currCycleTime) * _cycleHeightMultiplier;
        Shoot(bulletAngle);
        //Advance rotation
        _currCycleTime += Random.Range(_cycleMinSpeed, _cycleMaxSpeed);
        //if curve is finished start again
        if(_currCycleTime >= _curveLength)
            _currCycleTime = 0;
        KillBehaviour();
    }
    void Shoot(float bulletAngle)
    {
        bool flipVertically = _flipVerticallyIfPlayerIsLeft && _playerDirectionOnCurrCycle.x < 0;
        bool flipHorizontally = _flipHorizontallyIfPlayerIsDown && _playerDirectionOnCurrCycle.y < 0;

        bulletAngle = Utility.GetFlippedAngle(bulletAngle, flipVertically, flipHorizontally);

        Vector2 bulletStartDirection = new Vector2(Mathf.Cos(bulletAngle * Mathf.Deg2Rad), Mathf.Sin(bulletAngle * Mathf.Deg2Rad));
        Vector2 bulletLocalPos = new(bulletStartDirection.x * _firePointDist, bulletStartDirection.y * _firePointDist);

        Vector2 shootingPosition = (Vector2)EnemyControl.transform.position + bulletLocalPos;
        EnemyProyectile proyectile = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["EnemyProyectile"], shootingPosition, Quaternion.Euler(0, 0, bulletAngle)).GetComponent<EnemyProyectile>();
        proyectile.Initiate(_damage, _proyectileLifetime, _proyectileData, ApplyStatusEffects);
        
    }


    void ApplyStatusEffects(PlayerControl player)
    {
        foreach(var statusEffect in _appliedPlayerStatusOnHit)
        {
            player.StatusEffectManager.AddEffects(statusEffect.StatusEffects, statusEffect);

        }
    }
}
