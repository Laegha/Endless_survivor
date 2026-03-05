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

    public void DestroyObj(SupportObjectBehaviourManager destroyedObj, float destroyTime = 0)//ideally make this some kind of interface so it can be used with any object
    {
        GameManager.gm.DelayAction(destroyTime,() => {
            destroyedObj.Destroyed();
            Destroy(destroyedObj.gameObject);
        }, () => this == null);
        
    }
}
