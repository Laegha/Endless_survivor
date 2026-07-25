using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunEnemyOverTimeStatusEffect : EnemyStatusEffect
{
    new public static bool isUsable => true;
    [SerializeField] float _stunTime;
    [SerializeField] float _timeBetweenStuns;
    [SerializeField] bool _startStunned;
    float _stunTimer;
    float _stunCooldownTimer;
    static Dictionary<EnemyControl, int> _stunAmmountPerEnemy = new();
    public override void Initialize(EnemyControl affectedEnemyControl, EnemyStatusEffect original)
    {
        base.Initialize(affectedEnemyControl, original);
        var stunOverTimeOriginal = original as StunEnemyOverTimeStatusEffect;
        _stunTime = stunOverTimeOriginal._stunTime;
        _timeBetweenStuns = stunOverTimeOriginal._timeBetweenStuns;
        _startStunned = stunOverTimeOriginal._startStunned;
        if(!_startStunned) return;
        _stunTimer = _stunTime;
        AddStun();
        _stunCooldownTimer = -1;
    }
    public override void Update()
    {
        base.Update();
        if (_stunTimer > 0)
        {
            _stunTimer -= Time.deltaTime;
            AffectedEnemyControl.BehaviourManager.IsStunned = true;
            return;
        }
        if (_stunCooldownTimer <= 0)
        {
            _stunCooldownTimer = _timeBetweenStuns;
            RemoveStun();
        }
        if (_stunCooldownTimer > 0)
        {
            _stunCooldownTimer -= Time.deltaTime;
            if (_stunCooldownTimer <= 0)
            {
                AddStun();
                _stunTimer = _stunTime;
            }

        }

    }
    void AddStun()
    {
        if (_stunAmmountPerEnemy.ContainsKey(AffectedEnemyControl))
            _stunAmmountPerEnemy[AffectedEnemyControl]++;
        else
            _stunAmmountPerEnemy.Add(AffectedEnemyControl, 1);
        AffectedEnemyControl.RbForcesController.ChangeCurrForce(new(new(0, 0), 0, 10000, ForceMode2D.Impulse, 0));
        AffectedEnemyControl.BehaviourManager.IsStunned = true;
    }
    public void RemoveStun()
    {
        _stunAmmountPerEnemy[AffectedEnemyControl]--;
        if (_stunAmmountPerEnemy[AffectedEnemyControl] > 0)
            return;
        _stunAmmountPerEnemy.Remove(AffectedEnemyControl);
        AffectedEnemyControl.BehaviourManager.IsStunned = false;
    }
    public override void End()
    {
        base.End();
        RemoveStun();
    }
}
