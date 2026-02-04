using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New BiomeData", menuName = "ScriptableObjects/Map/Tile Decoration", order = 3)]
public class TileDecorationData : ScriptableObject
{
    [SerializeField] CustomAnimation _decorAnimation;
    [SerializeField] MapElementSize _decorSize;

    public CustomAnimation DecorationAnimation { get { return _decorAnimation; } }
    public MapElementSize DecorSize { get { return _decorSize; } }

}
