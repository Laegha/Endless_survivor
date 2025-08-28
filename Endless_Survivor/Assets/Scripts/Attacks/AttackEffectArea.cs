using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffectArea
{
    public enum IAttackEffectAreaType
    {
        Point,
        Vector,
        Square
    }
    public IAttackEffectAreaType type;
    public Vector2 start;
    public Vector2 end;
    public bool moves;

    public AttackEffectArea(IAttackEffectAreaType type, Vector2 start, Vector2 end, bool moves)
    {
        this.type = type;
        this.start = start;
        this.end = end;
        this.moves = moves;
    }
}
