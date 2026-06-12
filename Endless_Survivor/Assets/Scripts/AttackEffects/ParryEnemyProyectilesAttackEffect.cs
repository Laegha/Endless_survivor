using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Unity.VisualScripting;
using System.Linq;

public class ParryEnemyProyectilesAttackEffect : AttackEffect
{
    new public static bool isUsable => true;
    enum ParryEffectCheckTimeType
    {
        OnAttack,
        OnUpdate
    }
    enum ParryEffectCheckFormType
    {
        Area,
        AlignedRaycast
    }
    [SerializeField]ParryEffectCheckTimeType _checkTime;
    [SerializeField]float _ifUpdateTimeBetweenChecks;
    [SerializeField]ParryEffectCheckFormType _checkForm;
    [SerializeField]Vector2 _areaSize;
    [SerializeField]CapsuleDirection2D _areaDirection;
    [SerializeField][Tooltip("Negative for Infinity")]float _raycastRange;
    [SerializeField]LayerMask _parryingLayers;

    [SerializeField] CustomAnimation _parriedProyectileAnim; 

    Action _checkParriedProyectiles;
    float _updateTimer;

    public ParryEnemyProyectilesAttackEffect(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }

    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var parryOriginal = original as ParryEnemyProyectilesAttackEffect;
        _checkTime = parryOriginal._checkTime;
        _ifUpdateTimeBetweenChecks = parryOriginal._ifUpdateTimeBetweenChecks;
        _checkForm = parryOriginal._checkForm;
        _areaSize = parryOriginal._areaSize;
        _areaDirection = parryOriginal._areaDirection;
        _raycastRange = parryOriginal._raycastRange;
        _parryingLayers = parryOriginal._parryingLayers;

        _parriedProyectileAnim = new(null, parryOriginal._parriedProyectileAnim);

        switch(_checkForm)
        {
            case ParryEffectCheckFormType.Area:
                _checkParriedProyectiles += CheckArea;
                break;
            case ParryEffectCheckFormType.AlignedRaycast:
                _checkParriedProyectiles += CheckRaycast;
                break;
        }
        
        switch(_checkTime)
        {
            case ParryEffectCheckTimeType.OnAttack:
                OnAttack += _checkParriedProyectiles;
                break;
            case ParryEffectCheckTimeType.OnUpdate:
                OnUpdate += CheckParryPeriodically;
                break;
        }
    }

    void CheckParryPeriodically()
    {
        _updateTimer -= Time.deltaTime;
        if (_updateTimer > 0)
            return;
        _updateTimer = _ifUpdateTimeBetweenChecks;
        _checkParriedProyectiles?.Invoke();

    }

    void CheckArea()
    {
        var colsInArea = Physics2D.OverlapCapsuleAll(AffectedAttack.transform.position, _areaSize, _areaDirection, 0, _parryingLayers).ToList();
        var enemyProyectilesInArea = colsInArea.Where(x => x.GetComponent<EnemyProyectile>() != null).Select(x => x.GetComponent<EnemyProyectile>());
        foreach(var proyectile in  enemyProyectilesInArea)
        {
            proyectile.GetParried(_parriedProyectileAnim);
        }
    }
    void CheckRaycast()
    {
        Vector2 rayDir = new(Mathf.Cos(AffectedAttack.transform.rotation.z), Mathf.Sin(AffectedAttack.transform.rotation.z));
        var hitsInRay = Physics2D.RaycastAll(AffectedAttack.transform.position, rayDir, _raycastRange, _parryingLayers).ToList();
        var enemyProyectilesInArea = hitsInRay.Where(x => x.collider.GetComponent<EnemyProyectile>() != null).Select(x => x.collider.GetComponent<EnemyProyectile>());
        foreach (var proyectile in enemyProyectilesInArea)
        {
            proyectile.GetParried(_parriedProyectileAnim);
        }
    }
}
