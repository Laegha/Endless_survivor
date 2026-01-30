using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AttackEffect
{
    //activation requirements
    public static bool isUsable => false;
    [SerializeField] bool _usesSeparateChance = false;
    [SerializeField][Range(0, 100)] float _separateChance = 50;

    //activated values
    Attack _affectedAttack;
    Action _onAttack;
    Action _onUpdate;
    Action<EnemyControl> _onEnemyHit;
    //IAttackEffectSinergy _sinergy = null;
    public Attack AffectedAttack { get { return _affectedAttack; } }
    public bool UsesSeparateChance { get { return _usesSeparateChance; } }
    public float EffectChance { get { return _separateChance; } }
    public Action OnAttack{ get { return _onAttack; } set { _onAttack = value; } }
    public Action OnUpdate { get { return _onUpdate; } set { _onUpdate = value; } }
    public Action<EnemyControl> OnEnemyHit { get { return _onEnemyHit;} set { _onEnemyHit = value; } }

    public AttackEffect(AttackEffect original, Attack affectedAttack)
    {
        if (original != null)
            Initiate(original, affectedAttack);
    }
    public virtual void Initiate(AttackEffect original, Attack affectedAttack)
    {
        _affectedAttack = affectedAttack;
        
        //var types = Utility.GetSubclassesOf(typeof(AttackWithEffectSinergy<,>));
        //foreach (var type in types)
        //{
        //    if (type.GetProperty("attack").PropertyType != affectedAttack.GetType() || type.GetProperty("effect").PropertyType != GetType())
        //        continue;
        //    _sinergy = (IAttackEffectSinergy)Activator.CreateInstance(type);
        //    _sinergy.Initiate(affectedAttack, this);
        //    break;
        //}
    }
}
