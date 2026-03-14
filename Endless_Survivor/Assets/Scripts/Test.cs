using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    List<string> list = new List<string>();
    private void Update()
    {
        list.Remove("tst");
    }
}