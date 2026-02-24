using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomingAttackAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    [Range(0, 100)][SerializeField] float _angleCorrectionPercentage;
    [SerializeField] float _timeBetweenCorrection;

    Rigidbody2D _attackRb;
    float _timeSinceLastCorrection;

    public HomingAttackAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }

    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var homingOriginal = original as HomingAttackAttackEffect;
        _angleCorrectionPercentage = homingOriginal._angleCorrectionPercentage;
        _timeBetweenCorrection = homingOriginal._timeBetweenCorrection;
        _attackRb = AffectedAttack.GetComponent<Rigidbody2D>();
        if(_attackRb != null) 
            OnUpdate += CorrectAngleToNearestEnemy;
    }

    void CorrectAngleToNearestEnemy()
    {
        _timeSinceLastCorrection += Time.deltaTime;
        if (_timeSinceLastCorrection <= _timeBetweenCorrection)
            return;
        _timeSinceLastCorrection = 0;

        var closestEnemies = Utility.GetClosestTo(EnemySpawnManager.esm.Enemies, AffectedAttack.transform);
        if (closestEnemies.Count == 0)
            return;
        Vector2 attackVelocity = _attackRb.velocity;
        Vector2 attackDir = attackVelocity.normalized;
        float attackSpeed = attackVelocity.magnitude;

        Vector2 attackToEnemy = closestEnemies[0].transform.position - AffectedAttack.transform.position;
        float currAngle = Mathf.Atan2(attackDir.y, attackDir.x);
        float bestAngle = Mathf.Atan2(attackToEnemy.y, attackToEnemy.x);
        
        float maxRotation = bestAngle - currAngle;
        Debug.Log("MAX ROT " + maxRotation);
        float frameRotation = maxRotation * _angleCorrectionPercentage / 100;
        float newAngle = currAngle + frameRotation;
        Vector2 newDir = new Vector2(Mathf.Cos(newAngle), Mathf.Sin(newAngle));

        _attackRb.velocity = newDir.normalized * attackSpeed;
    }
}
