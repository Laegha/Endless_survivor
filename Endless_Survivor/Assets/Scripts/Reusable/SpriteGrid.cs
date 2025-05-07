using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteGrid : MonoBehaviour
{
    enum GridOrientation
    {
        Vertical, 
        Horizontal
    }
    [SerializeField] float _spacing;
    [SerializeField] GameObject _spritePrefab;
    List<GridSpriteInfo> _spritesInGrid;
    public void AddSpriteToGrid(Sprite sprite)
    {
        GridSpriteInfo addedSpriteInfo = new GridSpriteInfo();
        GameObject addedSpriteObject = Instantiate(_spritePrefab, transform);
        addedSpriteInfo.gameObject = addedSpriteObject;
        addedSpriteInfo.width = sprite.bounds.size.x;
        addedSpriteInfo.height = sprite.bounds.size.y;
        //scale added object to fit target size (indicated by prefab)
        //reorganize existing objects
        _spritesInGrid.Add(addedSpriteInfo);
    }
}

class GridSpriteInfo
{
    public GameObject gameObject;
    public float width;
    public float height;
}