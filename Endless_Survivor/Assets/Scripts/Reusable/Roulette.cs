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
            List<int> elementNumbers = new List<int>();
            for(int i = lastElementEnd; i < lastElementEnd + element.Value; i++)
                elementNumbers.Add(i);

            _roulette.Add(new RouletteElement<T>(element.Key, lastElementEnd, lastElementEnd + element.Value, elementNumbers));
            lastElementEnd += element.Value;
        }
        _rouletteTotalWeight = lastElementEnd;
    }

    public T Spin()
    {
        int rouletteResult = Random.Range(0, _rouletteTotalWeight);
        //var elementResult = _roulette.Find(element => element.minValue <= rouletteResult && element.maxValue >= rouletteResult);
        var elementResult = _roulette.Find(element => element.luckyNumbers.Contains(rouletteResult));  
        return elementResult != null ? elementResult.key : default;
    }
}

public class RouletteElement<T>
{
    public T key;
    public int minValue;
    public int maxValue;
    public List<int> luckyNumbers;

    public RouletteElement(T key, int minValue, int maxValue, List<int> luckyNumbers)
    {
        this.key = key;
        this.minValue = minValue;
        this.maxValue = maxValue;
        this.luckyNumbers = luckyNumbers;
    }
}