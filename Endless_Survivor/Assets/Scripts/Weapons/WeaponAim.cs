using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    WeaponAttackManager _weapon;
    PlayerStats _playerStats;
    SpriteRenderer _spriteRenderer;
    Transform _directionBase;
    Func<Vector2> _distCheckPosition;
    List<WeaponTarget> _attackTargets = new();

    RaycastHit2D _currTrackingEnemyHit;
    public RaycastHit2D CurrTrackingEnemyHit { get {  return _currTrackingEnemyHit; } }
    public List<WeaponTarget> AttackTargets {  get { return _attackTargets; } }
    List<WeaponTarget> TargetedObjs
    {
        get
        {
            var result = new List<WeaponTarget>(_attackTargets);
            foreach(var enemy in EnemySpawnManager.esm.Enemies)
            {
                result.Add(new(enemy, 0));
            }
            return result;
        }
    }

    private void Awake()
    {
        _directionBase = transform;
        _distCheckPosition = () => { return transform.position; };
    }
    private void Start()
    {
        _weapon = GetComponent<WeaponAttackManager>();
        _playerStats = PlayerControl.pc.PlayerStats;
        _spriteRenderer = GetComponent<WeaponControl>().Gfx;
    }

    void Update()
    {
        var attackTargetsCopy = new List<WeaponTarget>(_attackTargets);
        foreach(var attackTarget in attackTargetsCopy)
        { 
            if(attackTarget.obj == null)
            {
                _attackTargets.Remove(attackTarget);
            }
        }

        if (TargetedObjs.Count == 0)
            return;

        var targets = TargetedObjs;
        targets.Sort(new WeaponTargetComparer(PlayerControl.pc.transform, _weapon.WeaponStats.Range));
        if (AttackTargets.Count > 0)
        {
            print("Targeting " + targets[0].obj.transform.name);
            print("Should target: " + AttackTargets[0].obj.transform.name + " with priopiryty of " + AttackTargets[0].priority);
        }

        Transform closestTarget = targets[0].obj.transform;
        Vector2 direction = closestTarget.position - _directionBase.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (direction.x < 0)
            _spriteRenderer.flipY = true;
        else
            _spriteRenderer.flipY = false;

        Vector2 distDirection = (Vector2)closestTarget.position - _distCheckPosition();
        var closestTargetHit = Physics2D.Raycast(_distCheckPosition(), distDirection, Mathf.Infinity, Utility.GetCollidableLayers("PlayerAttack"));
        _currTrackingEnemyHit = closestTargetHit;
        float distance = closestTargetHit.distance;
        //print("DISTANCE WITH CLOSEST ENEMY: " + distance + ". CLOSEST ENEMY: " + closestEnemyHit.collider);
        if (distance <= _weapon.WeaponStats.Range)
        {
            _weapon.InRange = true;
            _weapon.TryAttack();
        }
        else
        {
            _weapon.InRange = false;

        }
    }
    public void ChangeDirectionBase(Transform newBase) => _directionBase = newBase;
    public void ChangeDistCheckPos(Func<Vector2> newPos) => _distCheckPosition = newPos;
}