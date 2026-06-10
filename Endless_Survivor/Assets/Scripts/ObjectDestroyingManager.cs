using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectDestroyingManager : MonoBehaviour
{
    static ObjectDestroyingManager instance;
    public static ObjectDestroyingManager odm
    {
        get
        {
            return instance;
        }
    }

    private void Awake()
    {
        instance = this;
    }

    public void DestroyObj(GameObject destroyedObj, Action onDestroyed = null, float destroyTime = 0)//ideally make this some kind of interface so it can be used with any object
    {
        GameManager.gm.DelayAction(destroyTime,() => {
            onDestroyed?.Invoke();
            Destroy(destroyedObj);
        }, () => this == null);
        
    }
}
