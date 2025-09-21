using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementIsNewInfo<T> where T : ScriptableObject
{
    public T element;
    public bool isNew;

    public ElementIsNewInfo(T element, bool isNew)
    {
        this.element = element;
        this.isNew = isNew;
    }
}
