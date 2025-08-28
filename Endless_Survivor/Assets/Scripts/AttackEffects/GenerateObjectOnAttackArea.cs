using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static AttackEffectArea.IAttackEffectAreaType;

public class GenerateObjectOnAttackArea : AttackEffect
{
    new public static bool isUsable => true;

    [SerializeField] GameObject _generatedObjPrefab;
    [SerializeField] float _distanceBetweenObjs;

    float _lapsedDistance;
    Vector2 _prevPosition;
    public GenerateObjectOnAttackArea(AttackEffect original, Attack affectedAttack) : base(original, affectedAttack) { }
    public override void Initiate(AttackEffect original, Attack affectedAttack)
    {
        base.Initiate(original, affectedAttack);
        var genreateObjOriginal = original as GenerateObjectOnAttackArea;
        _generatedObjPrefab = genreateObjOriginal._generatedObjPrefab;
        _distanceBetweenObjs = genreateObjOriginal._distanceBetweenObjs;
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
        GameObject.Instantiate(_generatedObjPrefab, position, Quaternion.identity);

    }
    void Update()
    {
        Vector2 deltaMovement = _prevPosition - AffectedAttack.AttackEffectArea.start;
        _lapsedDistance += deltaMovement.magnitude;
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
