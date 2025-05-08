using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] SpriteGrid grid;
    [SerializeField] Sprite[] sprites;

    private void Start()
    {
        foreach (var sprite in sprites)
            grid.AddSpriteToGrid(sprite);
    }

}
