using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDistComparer : IComparer<GameObject>
{
    Transform _comparingPoint;
    public ObjectDistComparer(Transform comparingPoint)
    {
        _comparingPoint = comparingPoint;
    }
    public int Compare(GameObject objectA, GameObject objectB)
    {
        float distA = Vector2.Distance(objectA.transform.position, _comparingPoint.position);
        float distB = Vector2.Distance(objectB.transform.position, _comparingPoint.position);

        return distA.CompareTo(distB);
    }
}
