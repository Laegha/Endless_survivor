using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "New BiomeData", menuName = "ScriptableObjects/Map/Tile SupportObj", order = 2)]
public class TileSupportObjData : ScriptableObject
{
    [SerializeField] SupportObjectData _supportObj;
    [SerializeField] MapElementSize _objSize;

    public SupportObjectData SupportObj {  get { return _supportObj; } }
    public MapElementSize ObjSize { get { return _objSize; } }
}
