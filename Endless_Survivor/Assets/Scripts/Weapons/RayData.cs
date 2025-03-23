using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RayData
{
    [SerializeField] Material _rayMaterial;
    [SerializeField] float _rayStartWidth;
    [SerializeField] float _rayEndWidth;

    public Material RayMaterial {  get { return _rayMaterial; } }
    public float RayStartWidth { get { return _rayStartWidth; } }
    public float RayEndWidth { get { return _rayEndWidth; } }
}
