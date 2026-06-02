using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CameraWorldSize
{
    public static Vector2 GetCameraHalfExtents(Camera cam)
    {
        if (cam.orthographic)
        {
            float halfHeight = cam.orthographicSize;
            float halfWidth = halfHeight * cam.aspect;
            return new Vector2(halfWidth, halfHeight);
        }
        else
        {
            return GetPerspectiveHalfExtentsAtDepth(cam, cam.nearClipPlane);
        }
    }
    public static Vector2 GetPerspectiveHalfExtentsAtDepth(Camera cam, float depth)
    {
        float halfHeight = depth * Mathf.Tan(cam.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float halfWidth = halfHeight * cam.aspect;
        return new Vector2(halfWidth, halfHeight);
    }
    public static Vector2 GetCameraFullSize(Camera cam)
    {
        return GetCameraHalfExtents(cam) * 2f;
    }
}
