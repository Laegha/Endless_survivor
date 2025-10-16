using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    Weapon _weapon;
    PlayerStats _playerStats;
    SpriteRenderer _spriteRenderer;
    Transform _directionBase;
    Func<Vector2> _distCheckPosition;

    private void Awake()
    {
        _directionBase = transform;
        _distCheckPosition = () => { return transform.position; };
    }
    private void Start()
    {
        _weapon = GetComponent<Weapon>();
        _playerStats = PlayerControl.pc.PlayerStats;
        _spriteRenderer = GetComponent<WeaponControl>().Gfx;
    }

    void Update()
    {
        if(WaveManager.wm.Enemies.Count == 0)
            return;
        
        Transform closestEnemy = Utility.GetClosestTo(WaveManager.wm.Enemies, PlayerControl.pc.transform)[0].transform;
        Vector2 direction = closestEnemy.position - _directionBase.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (direction.x < 0)
            _spriteRenderer.flipY = true;
        else
            _spriteRenderer.flipY = false;
        Vector2 distance = (Vector2)closestEnemy.position - _distCheckPosition();
        if (distance.magnitude <= _weapon.WeaponStats.Range)
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