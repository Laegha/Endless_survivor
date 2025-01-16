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
        _playerStats = GameManager.gm.player.GetComponent<PlayerStats>();
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(WaveManager.wm.Enemies.Count == 0)
            return;

        List<GameObject> enemies = new List<GameObject> (WaveManager.wm.Enemies);
        enemies.Sort(new EnemyDistComparer());

        Transform closestEnemy = enemies[0].transform;
        Vector2 distance = closestEnemy.position - GameManager.gm.player.position;
        Vector2 direction = distance.normalized;
        transform.rotation = Quaternion.Euler(direction);
        if(direction.x < 0)
            _spriteRenderer.flipX = true;
        else
            _spriteRenderer.flipX = false;

        if (distance.magnitude <= _playerStats.Range + _weapon.Range)
            _weapon.Attack();
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
