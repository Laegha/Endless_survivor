using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitAttackAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _splitAmmount = 2;
    [SerializeField] float _distBetweenSplits;
    public SplitAttackAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var splitOriginal = original as SplitAttackAttackEffect;
        _splitAmmount = splitOriginal._splitAmmount;
        _distBetweenSplits = splitOriginal._distBetweenSplits;

        OnAttack += SplitAttack;
    }
    void SplitAttack()
    {
        List<GameObject> enemies = EnemySpawnManager.esm.Enemies;
        var closestEnemies = Utility.GetClosestTo(enemies, AffectedAttack.transform);
        Vector2 splitAttackPos = AffectedAttack.transform.position;
        for (int i = 0; i < _splitAmmount; i++)
        {
            int targetEnemyIndex = i >= closestEnemies.Count ? Random.Range(0, closestEnemies.Count) : i;
            GameObject splitTargetedEnemy = Utility.GetClosestTo(enemies, AffectedAttack.transform)[targetEnemyIndex];
            Vector2 splitAttackDir = (splitTargetedEnemy.transform.position - AffectedAttack.transform.position).normalized;
            AffectedAttack.ParentWeapon.Attack(splitAttackPos, splitAttackDir, true);

            splitAttackPos = (Vector2)AffectedAttack.transform.position + (Vector2)AffectedAttack.transform.up * _distBetweenSplits * (i + 1) * Mathf.Pow(-1, i + 1);
        }
        GameObject.Destroy(AffectedAttack.gameObject);
    }
}
