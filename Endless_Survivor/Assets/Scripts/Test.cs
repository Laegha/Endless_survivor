using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    List<int> list = new List<int>() { 0, 1};
    List<int> list2 = new List<int>() { 2, 3 };

    private void Start()
    {
        print("TEST " + list.Count);
        print("TEST2 " + list.Concat(list2).Count());
        print("TEST3 " + list.Count);
    }

}
