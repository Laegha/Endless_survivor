using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager me;

    public static GameManager gm
    {
        get { return me; }
    }

    private void Awake()
    {
        if (me != null) 
        {
            Destroy(gameObject);
            return;
        }
        me = this;
        DontDestroyOnLoad(gameObject);
    }


}
