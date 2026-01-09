using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InterfaceHelper 
{
    public static List<Type> GetImplementationsOf<T>()
    {
        return UnityEditor.TypeCache.GetTypesDerivedFrom<T>()
            .Where(t => !t.IsAbstract && !t.IsInterface)
            .ToList();
    }
}
