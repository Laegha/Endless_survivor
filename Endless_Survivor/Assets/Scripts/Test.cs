using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Test : MonoBehaviour
{
    [SerializeField] LineXInfo _lineinf;
    [SerializeField] LineRenderer _line;
    LineXInfo lineinf;
    private void Start()
    {
        lineinf = new(_lineinf, Vector2.zero, Vector2.right, 5);
        lineinf.DrawLine(_line);
    }
    private void Update()
    {

    }
}