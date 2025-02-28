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
        _playerStats = GameManager.gm.player.GetComponent<PlayerStateMachine>().PlayerStats;
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(WaveManager.wm.Enemies.Count == 0)
            return;

        List<GameObject> enemies = new List<GameObject> (WaveManager.wm.Enemies);
        enemies.Sort(new EnemyDistComparer());

        Transform closestEnemy = enemies[0].transform;
        Vector2 direction = transform.position - closestEnemy.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);

        if (direction.x < 0)
            _spriteRenderer.flipY = true;
        else
            _spriteRenderer.flipY = false;

        Vector2 distance = closestEnemy.position - GameManager.gm.player.position;
        if (distance.magnitude <= _playerStats.Range + _weapon.WeaponStats.Range)
            _weapon.TryAttack();
    }
}

class EnemyDistComparer : IComparer<GameObject>
{
    public int Compare(GameObject enemyA, GameObject enemyB)
    {
        float distA = Mathf.Abs(GameManager.gm.player.position.magnitude - enemyA.transform.position.magnitude);
        float distB = Mathf.Abs(GameManager.gm.player.position.magnitude - enemyB.transform.position.magnitude);

        return distA.CompareTo(distB);
    }
}
