using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class AnimateTilesLineModifier : CustomLineModifierBase
{
    new public static int maxStacks => 1;

    [SerializeField] CustomLineAnimatedTile[] _tilesAnimations = new CustomLineAnimatedTile[0];
    [Header("If true, the tiles are generated in the order of the array, if not, at random")]
    [SerializeField] bool followTileOrder;

    List<CustomLineAnimatedTile> _animatedTiles = new();

    public override void CopyValues(CustomLineModifierBase original, LineXInfo lineXInfo)
    {
        base.CopyValues(original, lineXInfo);
        var animateTilesOriginal = original as AnimateTilesLineModifier;
        _tilesAnimations = new CustomLineAnimatedTile[animateTilesOriginal._tilesAnimations.Length];
        for (int i = 0; i < _tilesAnimations.Length; i++)
        {
            _tilesAnimations[i] = animateTilesOriginal._tilesAnimations[i];
        }
        OnStart += GiveAnimationsToTiles;
        OnUpdate += UpdateAnimations;
    }

    void GiveAnimationsToTiles()
    {
        int tileAnimationId = 0;
        foreach (var lineTile in LineInfo.LineTiles)
        {
            if(followTileOrder && tileAnimationId >= _tilesAnimations.Length)
                tileAnimationId = 0;
            int pickingAnimationId = followTileOrder ? tileAnimationId : UnityEngine.Random.Range(0, _tilesAnimations.Length);
            var tileAnimation = _tilesAnimations[pickingAnimationId];
            CustomLineAnimatedTile animatedTile = new(lineTile, tileAnimation.FramesPerSecond, tileAnimation.Frames);
            _animatedTiles.Add(animatedTile);
            tileAnimationId++;
        }
    }
    void UpdateAnimations()
    {
        foreach (var tile in _animatedTiles)
            tile.Update();
    }
}
