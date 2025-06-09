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
    [SerializeField] float _rayExitSpeed;

    public Material RayMaterial {  get { return _rayMaterial; } }
    public float RayStartWidth { get { return _rayStartWidth; } }
    public float RayEndWidth { get { return _rayEndWidth; } }
    public float RayExitSpeed { get { return _rayExitSpeed; } }

    public RayData(RayData original)
    {
        _rayMaterial = original._rayMaterial;
        _rayStartWidth = original._rayStartWidth;
        _rayEndWidth = original._rayEndWidth;
        _rayExitSpeed = original._rayExitSpeed;
    }
}
