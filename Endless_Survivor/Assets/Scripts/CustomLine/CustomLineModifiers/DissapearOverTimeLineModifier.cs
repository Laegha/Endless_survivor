using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DissapearOverTimeLineModifier : CustomLineModifierBase
{
    new public static int maxStacks => 1;
    [SerializeField] Material _tileDissapearingMaterial;
    [Tooltip("The speed IN UNITS at which the line dissapears")][SerializeField] float _lineLinearDissapearSpeed;
    [SerializeField] bool _dissapearOnStart;

    Func<float> _lineUnitsDissapearSpeed;//Making this a Func allows other scripts to edit the speed by something like x^2
    Func<float> _lineTileDissapearSpeed;
    List<Material> _tilesMaterials = new List<Material>();
    int _currDissapearingTile = 0;
    float _currTileDissapearThreshold = 0;
    public Func<float> LineDissapearSpeed { set { _lineUnitsDissapearSpeed = value; } }
    public override void CopyValues(CustomLineModifierBase original, LineXInfo lineXInfo)
    {
        base.CopyValues(original, lineXInfo);
        var dissapearOverTimeOriginal = original as DissapearOverTimeLineModifier;
        _tileDissapearingMaterial = dissapearOverTimeOriginal._tileDissapearingMaterial;
        _lineLinearDissapearSpeed = dissapearOverTimeOriginal._lineLinearDissapearSpeed;
        _dissapearOnStart = dissapearOverTimeOriginal._dissapearOnStart;
        _lineUnitsDissapearSpeed = () => _lineLinearDissapearSpeed;
        _lineTileDissapearSpeed = ConvertUnitsToTileSpeed;
        OnStart += GiveMaterialToTiles;
        if (_dissapearOnStart)
            OnStart += StartLineDissapear;
    }
    float ConvertUnitsToTileSpeed()
    {
        if (_lineUnitsDissapearSpeed == null)
            return 1;
        return _lineUnitsDissapearSpeed() / LineInfo.LineData.TileInfo.tileUnitRelevantLength; //speed / tile size
    }
    void GiveMaterialToTiles()
    {
        foreach (var lineTileRenderer in LineInfo.LineTiles)
        {
            Material newTileMaterial = new(_tileDissapearingMaterial);
            lineTileRenderer.material = newTileMaterial;
            newTileMaterial.SetFloat("_Angle", lineTileRenderer.transform.rotation.eulerAngles.z);
            _tilesMaterials.Add(newTileMaterial);
        }
    }
    public void StartLineDissapear()
    {
        OnUpdate += ProgressDissapearBySpeed;
        OnUpdate += UpdateMaterials;
    }
    void ProgressDissapearBySpeed()
    {
        _currTileDissapearThreshold += _lineTileDissapearSpeed() * Time.deltaTime;
        if (_currTileDissapearThreshold <= 1)
            return;
        _currTileDissapearThreshold = 0;
        _currDissapearingTile++;
    }

    void UpdateMaterials()
    {
        if (_currDissapearingTile >= _tilesMaterials.Count)
            return;
        for (int i = 0; i < _currDissapearingTile; i++)
        {
            _tilesMaterials[i].SetFloat("_Threshold", 1);
        }

        _tilesMaterials[_currDissapearingTile].SetFloat("_Threshold", _currTileDissapearThreshold);
    }
}
