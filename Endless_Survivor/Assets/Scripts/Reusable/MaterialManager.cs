using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> _renderers;
    MaterialOverride _currentOverride;
    List<MaterialOverride> _overridesQueue = new();   
    Dictionary<SpriteRenderer, Material> _defaultMaterials = new();

    private void Awake()
    {
        foreach (var renderer in _renderers)
        {
            _defaultMaterials.Add(renderer, renderer.material);
        }
    }

    public void SetMaterialOverride(MaterialOverride newMaterial)
    {
        _overridesQueue.Add(newMaterial);
        if(_currentOverride == null || newMaterial.authority > _currentOverride.authority)
            UpdateMaterials();
    }
    public void UnsetMaterialOverride(MaterialOverride unsetMaterial)
    {
        _overridesQueue.Remove(unsetMaterial);
        if(unsetMaterial == _currentOverride)
            UpdateMaterials();

    }
    void UpdateMaterials()
    {
        _overridesQueue.Sort(new MaterialAuthorityComparer());
        _currentOverride = _overridesQueue.LastOrDefault();
        foreach (var renderer in _renderers)
        {
            renderer.material = _currentOverride != null ? _currentOverride.material : _defaultMaterials[renderer];
        }
    }
    public void AddRenderer(SpriteRenderer renderer)
    {
        _renderers.Add(renderer);
        _defaultMaterials.Add(renderer, renderer.material);
    }
    public void ChangeDefaultMaterial(string rendererTransformPath, Material newMaterial)
    {
        var defaultMaterialsCopy = new Dictionary<SpriteRenderer, Material>(_defaultMaterials);
        foreach(var defaultRenderer in defaultMaterialsCopy)
        {
            if(Utility.GetTransfromPath(defaultRenderer.Key.transform) == rendererTransformPath)
            {
                _defaultMaterials[defaultRenderer.Key] = newMaterial;
                defaultRenderer.Key.material = newMaterial;
                return;
            }
        }
    }
    public void ChangeDefaultMaterial(Material newMaterial)
    {
        var defaultMaterialsCopy = new Dictionary<SpriteRenderer, Material>(_defaultMaterials);
        foreach (var renderer in defaultMaterialsCopy.Keys)
        {
            _defaultMaterials[renderer] = newMaterial;
            renderer.material = newMaterial;
        }

    }
    public void CleanRenderers()
    {
        foreach (var renderer in _renderers)
        {
            if(renderer == null)
                _defaultMaterials.Remove(renderer);
        }
        _renderers.RemoveAll(x => x == null);
    }
}
