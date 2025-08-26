using System.CodeDom.Compiler;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAim : MonoBehaviour
{
    Weapon _weapon;
    PlayerStats _playerStats;
    SpriteRenderer _spriteRenderer;

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

        List<GameObject> enemies = new List<GameObject> (WaveManager.wm.Enemies);
        enemies.Sort(new EnemyDistComparer(transform));

        Transform closestEnemy = enemies[0].transform;
        Vector2 direction = transform.position - closestEnemy.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (direction.x < 0)
            _spriteRenderer.flipY = true;
        else
            _spriteRenderer.flipY = false;

        Vector2 distance = closestEnemy.position - PlayerControl.pc.transform.position;
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
}

class EnemyDistComparer : IComparer<GameObject>
{
    Transform _comparingPoint;
    public EnemyDistComparer(Transform comparingPoint)
    {
        _comparingPoint = comparingPoint;
    }
    public int Compare(GameObject enemyA, GameObject enemyB)
    {
        float distA = Vector2.Distance(enemyA.transform.position, _comparingPoint.position);
        float distB = Vector2.Distance(enemyB.transform.position, _comparingPoint.position);
       
        return distA.CompareTo(distB);
    }
}
