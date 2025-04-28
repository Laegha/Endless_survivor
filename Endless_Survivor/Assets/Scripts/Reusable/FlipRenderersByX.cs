using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlipRenderersByX : MonoBehaviour
{
    [SerializeField] SpriteRenderer[] _affectedRenderers;

    private void LateUpdate()
    {
        float distY = PlayerControl.pc.transform.position.x - transform.position.x;
        if (distY > 0)
        {
            foreach (var renderer in _affectedRenderers)
                renderer.flipX = false;

        }
        else
        {
            foreach (var renderer in _affectedRenderers)
                renderer.flipX = true;

        }
    }
}
