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

    private void Awake()
    {
        _directionBase = transform;
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
        Vector2 direction = _directionBase.position - closestEnemy.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (direction.x < 0)
            _spriteRenderer.flipY = true;
        else
            _spriteRenderer.flipY = false;

        Vector2 distance = closestEnemy.position - transform.position;
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
}