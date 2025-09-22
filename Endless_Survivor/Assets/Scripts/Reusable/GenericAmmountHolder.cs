using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GenericAmmountHolder<T>
{
    public T generic;
    public int ammount = 1;
}
