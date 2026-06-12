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
    List<WeaponTarget> _exclusiveAttackTargets = new();
    static List<WeaponTarget> _sharedTargets = new();
    RaycastHit2D _currTrackingEnemyHit;
    
    
    public List<WeaponTarget> ExclusiveAttackTargets {  get { return _exclusiveAttackTargets; } }
    public static List<WeaponTarget> SharedAttackTargets {  get { return _sharedTargets; } }
    public RaycastHit2D CurrTrackingEnemyHit { get {  return _currTrackingEnemyHit; } }
    List<WeaponTarget> TargetedObjs
    {
        get
        {
            var result = new List<WeaponTarget>(_exclusiveAttackTargets).Concat(_sharedTargets).ToList();
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
        var attackTargetsCopy = new List<WeaponTarget>(_exclusiveAttackTargets);
        foreach(var attackTarget in attackTargetsCopy)
        { 
            if(attackTarget.obj == null)
            {
                _exclusiveAttackTargets.Remove(attackTarget);
            }
        }

        if (TargetedObjs.Count == 0)
            return;

        var targets = TargetedObjs;
        targets.Sort(new WeaponTargetComparer(PlayerControl.pc.transform, _weapon.WeaponStats.Range));

        Transform closestTarget = targets[0].obj.transform;
        Vector2 direction = closestTarget.position - _directionBase.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (direction.x < 0)
            _spriteRenderer.flipY = true;
        else
            _spriteRenderer.flipY = false;

        Vector2 distDirection = (Vector2)closestTarget.position - _distCheckPosition();
        var targetCol = closestTarget.GetComponent<Collider2D>();
        var allHitsInDir = Physics2D.RaycastAll(_distCheckPosition(), distDirection, Mathf.Infinity).ToList();
        var closestTargetHit = allHitsInDir.Find(hit => hit.collider.transform.root == closestTarget);
        if(!closestTargetHit)
        {
            Debug.LogError("The weapon " + transform.name + " targeted " + closestTarget.name + " but failed at casting a raycast");
            return;
        }
        _currTrackingEnemyHit = closestTargetHit;
        float distance = closestTargetHit.distance;
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