using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class CustomLineRenderer : MonoBehaviour
{
    List<CustomLineAnimatedTile> _tiles = new();
    public void GenerateLine(Vector2 startPosition, Vector2 endPosition, CustomLineInterface lineInterface)//sprite size in pixels
    {
        float tileSize = lineInterface.isVertical ? lineInterface.tileSize.y / 32 : lineInterface.tileSize.x / 32;

        Vector2 lineVector = endPosition - startPosition;
        float lineLength = lineVector.magnitude;
        int totalTiles = (int)(lineLength / tileSize);

        Vector2 lineDirection = lineVector.normalized;
        Vector2 firstTilePosition = startPosition + lineDirection * tileSize / 2;//we assume that the sprite's pivot point is centered
        float tileAngle = Mathf.Atan2(lineDirection.y, lineDirection.x);

        for (int i = 0; i < totalTiles; i++)
        {
            //tile GameObject generation and transform set
            var tileObj = new GameObject("LineTile " + i);
            tileObj.transform.position = firstTilePosition + lineDirection * tileSize * i;
            tileObj.transform.rotation = Quaternion.Euler(0, 0, tileAngle * Mathf.Rad2Deg - 90);
            tileObj.transform.parent = transform;

            //get tile data
            int tileIndex;
            if (!lineInterface.followTileOrder)
                tileIndex = UnityEngine.Random.Range(0, lineInterface.tiles.Length);
            else
                tileIndex = i - lineInterface.tiles.Length * (int)i / lineInterface.tiles.Length;

            var tileData = lineInterface.tiles[tileIndex];
            Sprite[] tileFrames = tileData.Frames;
            float tileFramesPerSecond = tileData.FramesPerSecond;
            var tileRenderer = tileObj.AddComponent<SpriteRenderer>();
            var tileSpriteSorter = tileObj.AddComponent<SortingOrderByY>();
            tileSpriteSorter.AffectedRenderers = new SpriteRenderer[] { tileRenderer };
            //create tile object and add it to the list
            _tiles.Add(new(tileRenderer, tileFramesPerSecond, tileFrames));
        }
    }

    private void Update()
    {
        foreach(var tile in _tiles)
            tile.Update();
    }
}
