using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MaterialManager : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> _renderers;
    Dictionary<SpriteRenderer, Material> _defaultMaterials = new();
    List<MaterialOverride> _overridesQueue = new();
    Dictionary<SpriteRenderer, Sprite> _sprites = new();

    private void Awake()
    {
        foreach (var renderer in _renderers)
        {
            _defaultMaterials.Add(renderer, renderer.material);
            _sprites.Add(renderer, null);
        }
    }
    private void Update()
    {
        UpdateMaterials();
    }
    public void SetMaterialOverride(MaterialOverride newMaterial)
    {
        _overridesQueue.Add(newMaterial);
        SortMaterials();
    }
    public void UnsetMaterialOverride(MaterialOverride unsetMaterial)
    {
        _overridesQueue.Remove(unsetMaterial);
        SortMaterials();

    }
    void SortMaterials() => _overridesQueue.Sort(new MaterialAuthorityComparer());
    void UpdateMaterials()
    {
        foreach (var renderer in _renderers)
        {
            if (_sprites[renderer] == renderer.sprite)
                continue;
            //get input texture
            Texture2D inputTex = renderer.sprite.texture;
            Rect textureRect = renderer.sprite.rect;

            //create an output texture
            RenderTexture tempRT = RenderTexture.GetTemporary(inputTex.width, inputTex.height, 0, RenderTextureFormat.ARGB64);
            Graphics.SetRenderTarget(tempRT);
            RenderTexture.active = tempRT;
            Texture2D outputTex = inputTex;
            //apply materials to output rt
            foreach (var matOverride in _overridesQueue)
            {
                GL.Clear(true, true, Color.clear);

                Graphics.Blit(outputTex, tempRT, matOverride.material);

                //extract the texture from the RT with the new material
                Texture2D extractedTex = new(tempRT.width, tempRT.height);
                extractedTex.ReadPixels(textureRect, (int)textureRect.x, (int)textureRect.y);
                extractedTex.Apply();

                outputTex = extractedTex;
            }
            RenderTexture.active = null;
            outputTex.filterMode = inputTex.filterMode;
            RenderTexture.ReleaseTemporary(tempRT);

            //create sprite with outputTex and apply it
            Vector2 pivot = renderer.sprite.pivot;
            Sprite newSprite = Sprite.Create(outputTex, textureRect, new Vector2(pivot.x / textureRect.width, pivot.y / textureRect.height), renderer.sprite.pixelsPerUnit);
            newSprite.name = renderer.sprite.name;
            renderer.sprite = newSprite;
            _sprites[renderer] = renderer.sprite;

        }
    }
    public void AddRenderer(SpriteRenderer renderer)
    {
        _renderers.Add(renderer);
        _defaultMaterials.Add(renderer, renderer.material);
        _sprites.Add(renderer, null);
    }
    public void CleanRenderers()
    {
        foreach (var renderer in _renderers)
        {
            if (renderer == null)
            {
                _defaultMaterials.Remove(renderer);
            }
        }
        _renderers.RemoveAll(x => x == null);
    }
}