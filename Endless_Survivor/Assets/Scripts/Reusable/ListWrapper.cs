using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ListWrapper<T>
{
    [SerializeField] List<T> _list;
    public List<T> List { get { return _list; } }
    public ListWrapper(List<T> list)
    {
        _list = new(list);
    }
}
