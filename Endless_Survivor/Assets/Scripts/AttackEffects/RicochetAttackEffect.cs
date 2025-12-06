using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RicochetAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _ricochetAmmount;//for now it's only one ricochet, until i figure out a way to stack different effects values into one single pool
    public RicochetAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var ricochetOriginal = original as RicochetAttackEffect;
        _ricochetAmmount = ricochetOriginal._ricochetAmmount;
        OnEnemyHit += GenerateRicochetAttack;
    }
    void GenerateRicochetAttack(EnemyControl hitEnemy)
    {
        Vector2 attackDir = hitEnemy.transform.position - AffectedAttack.transform.position;
        Vector2 hitPoint = Physics2D.Raycast(AffectedAttack.transform.position, attackDir, Mathf.Infinity, Utility.GetCollidableLayers("PlayerAttack")).point;
        
        List<GameObject> enemies = WaveManager.wm.Enemies;
        GameObject ricochetTargetedEnemy = Utility.GetClosestTo(enemies, hitEnemy.transform)[1];
        Vector2 ricochetAttackDir = (Vector2)ricochetTargetedEnemy.transform.position - hitPoint;
        Debug.Log("Ricochet Dist " +  ricochetAttackDir);
        List<Collider2D> ignoreColliders = hitEnemy.transform.root.GetComponentsInChildren<Collider2D>().ToList();
        AffectedAttack.ParentWeapon.Attack(hitPoint, ricochetAttackDir.normalized, true, ignoreColliders);
    }
}
