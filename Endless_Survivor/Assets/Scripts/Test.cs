using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField]CustomLineInterface lineInterface;
    [SerializeField] CustomLineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer.GenerateLine(Vector2.zero, Vector2.up * 5, lineInterface);
    }

}
