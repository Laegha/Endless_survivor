using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Roulette<T>
{
    List<RouletteElement<T>> _roulette = new List<RouletteElement<T>>();
    int _rouletteTotalWeight = 0;

    public Roulette(Dictionary<T, int> elements)
    {
        int lastElementEnd = 0;
        foreach (var element in elements)
        {
            _roulette.Add(new RouletteElement<T>(element.Key, lastElementEnd, lastElementEnd + element.Value));
            _rouletteTotalWeight += element.Value;
            lastElementEnd += element.Value;
        }

    }

    public T Spin()
    {
        int rouletteResult = Random.Range(0, _rouletteTotalWeight);
        var elementResult = _roulette.Find(element => element.minValue <= rouletteResult && element.maxValue >= rouletteResult);
        return elementResult != null ? elementResult.key : default;
    }
}

public class RouletteElement<T>
{
    public T key;
    public int minValue;
    public int maxValue;

    public RouletteElement(T key, int minValue, int maxValue)
    {
        this.key = key;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}