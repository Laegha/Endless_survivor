using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestructor : MonoBehaviour
{
    public void DestroyObj(int time)
    {
        Destroy(gameObject, time);
    }
}
