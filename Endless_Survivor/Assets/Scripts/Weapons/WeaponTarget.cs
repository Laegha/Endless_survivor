using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTarget 
{
    public GameObject obj;
    public int priority;

    public WeaponTarget(GameObject obj, int priority)
    {
        this.obj = obj;
        this.priority = priority;
    }
}
public class WeaponTargetComparer : IComparer<WeaponTarget>
{
    Transform _comparingPoint;
    float _maxDist;
    public WeaponTargetComparer(Transform comparingPoint, float maxDist)
    {
        _comparingPoint = comparingPoint;
        _maxDist = maxDist;
    }
    public int Compare(WeaponTarget targetA, WeaponTarget targetB)
    {
        float distA = Vector2.Distance(targetA.obj.transform.position, _comparingPoint.position);
        float distB = Vector2.Distance(targetB.obj.transform.position, _comparingPoint.position);

        if(distA > _maxDist)
            return 1;
        if (distB > _maxDist) 
            return -1;

        if (targetA.priority != targetB.priority)
            return -targetA.priority.CompareTo(targetB.priority);

        return distA.CompareTo(distB);
    }
}