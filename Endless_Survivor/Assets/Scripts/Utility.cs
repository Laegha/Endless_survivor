using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public static class Utility
{
    public static List<Type> GetSubclassesOf(Type baseType)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseType))
            .ToList();
    }
    public static int CountOccurrences(string text, string substring)
    {
        if (string.IsNullOrEmpty(substring)) return 0;

        int count = 0;
        int index = 0;

        while ((index = text.IndexOf(substring, index, StringComparison.OrdinalIgnoreCase)) != -1)
        {
            count++;
            index += substring.Length;
        }

        return count;
    }
    public static Vector2 GetPointInCircle(float radius, float angle)
    {
        float xPos = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        float yPos = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        return new Vector2(xPos, yPos);
    }
}
