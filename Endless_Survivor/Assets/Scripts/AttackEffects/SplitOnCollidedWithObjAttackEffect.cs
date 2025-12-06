using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SplitOnCollidedWithObjAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [SerializeField] string _objectId;
    [SerializeField] bool _destroyCollidedObj;
    [SerializeField] float _collisionAreaRadius = .25f;
    [SerializeField] int _splitAmmount = 2;
    [SerializeField] float _distBetweenSplits;
    public SplitOnCollidedWithObjAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var splitOnColOriginal = original as SplitOnCollidedWithObjAttackEffect;
        _objectId = splitOnColOriginal._objectId;
        _destroyCollidedObj = splitOnColOriginal._destroyCollidedObj;
        _collisionAreaRadius = splitOnColOriginal._collisionAreaRadius;
        _splitAmmount = splitOnColOriginal._splitAmmount;

        OnUpdate += CheckCollisionArea;
    }
    void CheckCollisionArea()
    {
        var objsInArea = Physics2D.OverlapCircleAll(AffectedAttack.transform.position, _collisionAreaRadius);
        foreach(var obj in objsInArea)
        {
            var collidedObjId = ObjectIdentifier.GetObjId(obj.gameObject);
            if(collidedObjId == null || !collidedObjId.Contains(_objectId)) 
                continue;
            if(_destroyCollidedObj)
                GameObject.Destroy(obj.gameObject);
            SplitAttack();
            OnUpdate -= CheckCollisionArea;
        }
    }
    void SplitAttack()
    {
        List<GameObject> enemies = WaveManager.wm.Enemies;
        var closestEnemies = Utility.GetClosestTo(enemies, AffectedAttack.transform);
        Vector2 splitAttackPos = AffectedAttack.transform.position;
        for (int i = 0; i < _splitAmmount; i++)
        {
            int targetEnemyIndex = i > closestEnemies.Count ? Random.Range(0, closestEnemies.Count) : i;
            GameObject splitTargetedEnemy = Utility.GetClosestTo(enemies, AffectedAttack.transform)[targetEnemyIndex];
            Vector2 splitAttackDir =( splitTargetedEnemy.transform.position - AffectedAttack.transform.position).normalized;
            AffectedAttack.ParentWeapon.Attack(splitAttackPos, splitAttackDir, true);

            splitAttackPos = (Vector2)AffectedAttack.transform.position + (Vector2)AffectedAttack.transform.up * _distBetweenSplits * (i + 1) * Mathf.Pow(-1, i+1);
        }
        GameObject.Destroy(AffectedAttack.gameObject);
    }
}
