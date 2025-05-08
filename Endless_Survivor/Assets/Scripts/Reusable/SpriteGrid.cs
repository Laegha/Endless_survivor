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
    [SerializeField] GridOrientation orientation = GridOrientation.Horizontal;
    [SerializeField] float _spacing;
    [SerializeField] SpriteRenderer _spritePrefab;
    List<GridSpriteInfo> _spritesInGrid;
    public GridSpriteInfo AddSpriteToGrid(Sprite sprite)
    {
        GridSpriteInfo addedSpriteInfo = new GridSpriteInfo();
        SpriteRenderer addedSpriteRenderer = Instantiate(_spritePrefab, transform);
        addedSpriteInfo.transform = addedSpriteRenderer.transform.root;
        addedSpriteInfo.width = sprite.bounds.size.x;
        addedSpriteInfo.height = sprite.bounds.size.y;

        //reorganize existing objects
        UpdateGridSpritesPositions();
        _spritesInGrid.Add(addedSpriteInfo);
        return addedSpriteInfo;
    }
    public void RemoveSpriteFromGrid(GridSpriteInfo removedSprite)
    {
        Destroy(removedSprite.transform);
        _spritesInGrid.Remove(removedSprite);
        UpdateGridSpritesPositions();
    }
    void UpdateGridSpritesPositions()
    {
        float totalGridLength = _spritesInGrid.Count * _spacing;
        _spritesInGrid.ForEach(x => totalGridLength += orientation == GridOrientation.Vertical ? x.height : x.width);
        Vector2 placingDir = (orientation == GridOrientation.Vertical ? Vector2.down : Vector2.right);
        Vector2 startPosition = (totalGridLength/2) * -placingDir;
        float offset = 0;
        for(int i = 0; i < _spritesInGrid.Count; i++)
        {
            _spritesInGrid[i].transform.localPosition = startPosition + offset * placingDir;
            offset += _spacing + (totalGridLength += orientation == GridOrientation.Vertical ? _spritesInGrid[i].height : _spritesInGrid[i].width) / 2;
        }
    }
}

public class GridSpriteInfo
{
    public Transform transform;
    public float width;
    public float height;
}