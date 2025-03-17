using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Roulette
{
    List<RouletteElement> _roulette = new List<RouletteElement>();
    float _rouletteTotalWeight = 0;
    public Roulette(Dictionary<dynamic, float> elements)
    {
        float lastElementEnd = 0;
        foreach(var element in elements)
        {
            _roulette.Add(new RouletteElement(element.Key, lastElementEnd, lastElementEnd + element.Value));
            _rouletteTotalWeight += element.Value;
            lastElementEnd += element.Value;
        }

    }

    public dynamic Spin()
    {
        float rouletteResult = Random.Range(0, _rouletteTotalWeight);
        return _roulette.Where(element => element.minValue <= rouletteResult && element.maxValue >= rouletteResult).ToList()[0].key;
    }
}

public class RouletteElement
{
    public dynamic key;
    public float minValue;
    public float maxValue;

    public RouletteElement(dynamic key, float minValue, float maxValue)
    {
        this.key = key;
        this.minValue = minValue;
        this.maxValue = maxValue;
    }
}