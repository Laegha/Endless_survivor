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
    List<GridSpriteInfo> _spritesInGrid = new();
    public GridSpriteInfo AddSpriteToGrid(Sprite sprite)
    {
        GridSpriteInfo addedSpriteInfo = new GridSpriteInfo();
        SpriteRenderer addedSpriteRenderer = Instantiate(_spritePrefab, transform);
        addedSpriteInfo.transform = addedSpriteRenderer.transform;
        addedSpriteInfo.width = sprite.bounds.size.x;
        addedSpriteInfo.height = sprite.bounds.size.y;
        addedSpriteRenderer.sprite = sprite;

        _spritesInGrid.Add(addedSpriteInfo);
        //reorganize existing objects
        UpdateGridSpritesPositions();
        return addedSpriteInfo;
    }
    public void RemoveSpriteFromGrid(GridSpriteInfo removedSprite)
    {
        Destroy(removedSprite.transform.gameObject);
        _spritesInGrid.Remove(removedSprite);
        UpdateGridSpritesPositions();
    }
    void UpdateGridSpritesPositions()
    {
        float totalGridLength = (_spritesInGrid.Count-1) * _spacing;
        _spritesInGrid.ForEach(x => totalGridLength += orientation == GridOrientation.Vertical ? x.height : x.width);
        Vector2 placingDir = (orientation == GridOrientation.Vertical ? Vector2.down : Vector2.right);
        Vector2 startPosition = (totalGridLength/2) * -placingDir;
        float offset = 0;
        for(int i = 0; i < _spritesInGrid.Count; i++)
        {
            float size = (orientation == GridOrientation.Vertical ? _spritesInGrid[i].height : _spritesInGrid[i].width);
            _spritesInGrid[i].transform.localPosition = startPosition + offset * placingDir + size * placingDir;
            offset += _spacing + size / 2;
        }
    }
}

public class GridSpriteInfo
{
    public Transform transform;
    public float width;
    public float height;
}