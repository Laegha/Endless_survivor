using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackEffectArea.IAttackEffectAreaType;

public class GenerateObjectOnAttackArea : AttackEffect
{
    new public static bool isUsable => true;

    [SerializeField] SupportObjectData _generatedSupportObj;
    [SerializeField] float _firstObjDist;
    [SerializeField] float _distanceBetweenObjs;

    float _lapsedDistance;
    Vector2 _prevPosition;
    public GenerateObjectOnAttackArea(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var genreateObjOriginal = original as GenerateObjectOnAttackArea;
        _generatedSupportObj = genreateObjOriginal._generatedSupportObj;
        _distanceBetweenObjs = genreateObjOriginal._distanceBetweenObjs;
        _lapsedDistance = _firstObjDist;
        switch(affectedAttack.AttackEffectArea.type)
        {
            case Point:
                if(AffectedAttack.AttackEffectArea.moves)
                {
                    _prevPosition = AffectedAttack.AttackEffectArea.start;
                    OnUpdate += Update;
                }
                else
                {
                    OnAttack += GenerateAcrossLength;
                }
                break;
            case Vector:
                OnAttack += GenerateAcrossLength;
                break;
            case Square:
                OnAttack += GenerateAcrossLength;
                break;
        }
    }

    void GenerateObj(Vector2 position)
    {
        Utility.GenerateSupportObj(_generatedSupportObj, position, Quaternion.identity);
    }
    void Update()
    {
        Vector2 deltaMovement = _prevPosition - AffectedAttack.AttackEffectArea.start;
        _prevPosition = AffectedAttack.AttackEffectArea.start;
        _lapsedDistance -= deltaMovement.magnitude;
        if(_lapsedDistance <= 0)
        {
            _lapsedDistance = _distanceBetweenObjs;
            GenerateObj(AffectedAttack.AttackEffectArea.start);
        }
    }
    void GenerateAcrossLength()
    {
        var vectorStartEnd = AffectedAttack.AttackEffectArea.end - AffectedAttack.AttackEffectArea.start;
        var attackLength = vectorStartEnd.magnitude;
        var attackDir = vectorStartEnd.normalized;
        int objectsToGenerate = (int)(attackLength / _distanceBetweenObjs);
        for(int i = 0; i <= objectsToGenerate; i++)
        {
            GenerateObj(AffectedAttack.AttackEffectArea.start + attackDir * i);
        }
    }
}
