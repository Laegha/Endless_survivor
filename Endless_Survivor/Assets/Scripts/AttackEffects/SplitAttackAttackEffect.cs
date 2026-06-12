using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitAttackAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] int _splitAmmount = 2;
    [SerializeField] float _distBetweenSplits;
    [SerializeField] bool _eachAttackTargetsClosest = true;
    [SerializeField] float _directionAngleAmplitude;
    public SplitAttackAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var splitOriginal = original as SplitAttackAttackEffect;
        _splitAmmount = splitOriginal._splitAmmount;
        _distBetweenSplits = splitOriginal._distBetweenSplits;
        _eachAttackTargetsClosest = splitOriginal._eachAttackTargetsClosest;
        _directionAngleAmplitude = splitOriginal._directionAngleAmplitude;

        OnAttack += SplitAttack;
    }
    void SplitAttack()
    {
        List<GameObject> enemies = EnemySpawnManager.esm.Enemies;
        var closestEnemies = Utility.GetClosestTo(enemies, AffectedAttack.transform);
        Vector2 splitAttackPos = AffectedAttack.transform.position;
        for (int i = 0; i < _splitAmmount; i++)
        {
            splitAttackPos = (Vector2)AffectedAttack.transform.position + (Vector2)AffectedAttack.transform.up * _distBetweenSplits * i * Mathf.Pow(-1, i + 1);
            if(_eachAttackTargetsClosest)
            {
                int targetEnemyIndex = i >= closestEnemies.Count ? Random.Range(0, closestEnemies.Count) : i;
                GameObject splitTargetedEnemy = Utility.GetClosestTo(enemies, AffectedAttack.transform)[targetEnemyIndex];
                Vector2 splitAttackDir = (splitTargetedEnemy.transform.position - AffectedAttack.transform.position).normalized;
                AffectedAttack.ParentWeapon.Attack(splitAttackPos, splitAttackDir, true, out _);//could actually use the new attack

            }
            else
            {
                float originalRotation = AffectedAttack.transform.rotation.eulerAngles.z;
                float splitAttackRotation = originalRotation + _directionAngleAmplitude / _splitAmmount * i * Mathf.Pow(-1, i + 1);
                Vector2 splitAttackDir = new Vector2(Mathf.Cos(splitAttackRotation * Mathf.Deg2Rad), Mathf.Sin(splitAttackRotation * Mathf.Deg2Rad));
                AffectedAttack.ParentWeapon.Attack(splitAttackPos, splitAttackDir, true, out _);//could actually use the new attack
            }

        }
        GameObject.Destroy(AffectedAttack.gameObject);
    }
}
