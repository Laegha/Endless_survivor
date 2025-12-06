using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class ObjectIdentifier 
{
    static Dictionary<string, GameObject> identifiedAttacks = new Dictionary<string, GameObject>();
    public static void AddIdentifiedObject(string id, GameObject obj)
    { 
        int idCount = identifiedAttacks.Where(x => x.Key.Contains(id)).Count();
        identifiedAttacks.Add(id + idCount, obj);
    }
    public static string GetObjId(GameObject obj)
    {
        return identifiedAttacks.Keys.ToList().Find(key => identifiedAttacks[key] == obj);
    }
}
