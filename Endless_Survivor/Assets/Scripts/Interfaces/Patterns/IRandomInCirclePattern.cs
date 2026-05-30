using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IRandomInCirclePattern : IPattern
{
    [SerializeField] float _circleRadiusMin = 0;
    [SerializeField] float _circleRadiusMax = 1;
    [SerializeField] float _minDistBetweenElements;

    int _triesToFindPosWithinDist = 1000;

    public IEnumerable<Vector2> GetPositions(Vector2 origin, int count)
    {
        List<Vector2> result = new List<Vector2>();
        for(int i = 0; i < count; i++)
        {
            var position = Random.insideUnitCircle * Random.Range(_circleRadiusMin, _circleRadiusMax);
            int errorCounter = 0;
            while(IsTooClose(position, result))
            {
                position = Random.insideUnitCircle * Random.Range(_circleRadiusMin, _circleRadiusMax);
                errorCounter++;
                if(errorCounter >= _triesToFindPosWithinDist)
                    return result;
            }
            
            result.Add(position);
        }
        return result;
    }

    bool IsTooClose(Vector2 checkingPoint, List<Vector2> previousPoints)
    {
        foreach(var point in previousPoints)
        {
            if(Vector2.Distance(checkingPoint, point) < _minDistBetweenElements) return true;
        }
        return false;
    }
}
