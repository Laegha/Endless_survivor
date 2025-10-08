using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    //[SerializeField]CustomLineInterface lineInterface;
    //[SerializeField] CustomLineRenderer lineRenderer;
    [SerializeField] Sprite sprite1;
    [SerializeField] Sprite sprite2;

    private void Start()
    {
        //lineRenderer.GenerateLine(Vector2.zero, Vector2.up * 5, lineInterface);
        print("SPRITE1 CENTER: " + sprite1.bounds.size);
        print("SPRITE2 CENTER: " + sprite2.bounds.size);
    }

}
