using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RouletteElementChance<T>
{
    [SerializeField] T _element;
    [SerializeField] int _chance;
    public T Element {  get { return _element; } }
    public int Chance { get { return _chance; } set { _chance = value; } }

    public RouletteElementChance(T element, int chance)
    {
        _element = element;
        _chance = chance;
    }
}

public class RouletteElementKey<T>
{
    public T element;
    public RouletteElementKey(T data)
    {
        this.element = data;
    }
}