using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;
using UnityEngine.UI;

public static class Utility
{
    public static List<Type> GetSubclassesOf(Type baseType)
    {
        return AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => type.IsClass && !type.IsAbstract && type.IsSubclassOf(baseType))
            .ToList();
    }
    public static int CountOccurrences(string text, string substring)
    {
        if (string.IsNullOrEmpty(substring)) return 0;

        int count = 0;
        int index = 0;

        while ((index = text.IndexOf(substring, index, StringComparison.OrdinalIgnoreCase)) != -1)
        {
            count++;
            index += substring.Length;
        }

        return count;
    }
    public static Vector2 GetPointInCircle(float radius, float angle)
    {
        float xPos = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;
        float yPos = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
        return new Vector2(xPos, yPos);
    }
    public static float GetAngleFromPointInCircle(Vector2 point, bool clockwise = true)
    {
        float angle = Mathf.Atan2(clockwise ? point.x : point.y, clockwise ? point.y : point.x) * Mathf.Rad2Deg;
        if (angle < 0)
            angle += 360f;

        return angle;
    }
    public static void ScaleImageToFitTarget(RectTransform imageTr, Sprite sprite, Vector2 targetSpace)
    {
        if (imageTr == null || sprite == null)
        {
            Debug.LogError("Image or Sprite is null!");
            return;
        }
        
        float spriteWidth = sprite.bounds.size.x;
        float spriteHeight = sprite.bounds.size.y;
        
        float targetWidth = targetSpace.x;
        float targetHeight = targetSpace.y;

        float sizeScaleFactor = Mathf.Min(targetWidth / spriteWidth, targetHeight / spriteHeight);

        float finalWidth = spriteWidth * sizeScaleFactor ;
        float finalHeight = spriteHeight * sizeScaleFactor ;

        imageTr.sizeDelta = new Vector2(finalWidth, finalHeight);
        
    }
    public static float GetLineLength(LineRenderer line)
    {
        float distance = 0;
        for(int i = 1; i < line.positionCount; i++)
        {
            float localDist = (line.GetPosition(i) - line.GetPosition(i - 1)).magnitude;
            distance += localDist;
        }
        return distance;
    }
    public static LayerMask GetCollidableLayers(string layerName)
    {
        int targetLayer = LayerMask.NameToLayer(layerName);
        int mask = 0;

        for (int i = 0; i < 32; i++)
        {
            // If collision between targetLayer and layer i is enabled
            if (!Physics2D.GetIgnoreLayerCollision(targetLayer, i))
            {
                mask |= (1 << i);
            }
        }

        return mask;
    }
    public static T FindFirstComponentInParent<T>(GameObject obj) where T : Component
    {
        T foundComponent = obj.GetComponent<T>();
        Transform checkingParent = obj.transform;
        while(checkingParent.root != checkingParent && foundComponent == null)
        {
            checkingParent = checkingParent.parent;
            foundComponent = checkingParent.GetComponent<T>();
        }
        return foundComponent;
    }

    public static AnimationClip GetClipFromAnimator(Animator animator, string clipName)
    {
        return animator.runtimeAnimatorController.animationClips.ToList().Find(clip => clip.name == clipName);
    }
    public static string GetTransfromPath(Transform tr)
    {
        string path = string.Empty;
        Transform currTr = tr;
        while (currTr != tr.root)
        {
            path = currTr.name + "/" + path;
            currTr = currTr.parent;
        }
        return path;
    }
    public static float ChangeFloatDecimals(float num, int decimals)
    {
        int escalated = (int)(num * Mathf.Pow(10, decimals));
        float decimatedNum = escalated / Mathf.Pow(10, decimals);
        return decimatedNum;
    }
    public static List<GameObject> GetClosestTo(List<GameObject> comparingObjects, Transform comparingTo)
    {
        List<GameObject> objs = new List<GameObject>(comparingObjects);
        objs.Sort(new ObjectDistComparer(comparingTo));
        return objs;
    }
    public static Vector3 TouchPosition()
    {
        Vector3 Return = Vector3.zero;

#if UNITY_IOS || UNITY_ANDROID && !UNITY_EDITOR
            Return = Input.GetTouch(0).position;
#else
            Return = Input.mousePosition;
#endif
        return Return;
    }
    public static Vector2 ScreenSize { get { return new Vector2(Screen.width, Screen.height); } }
    public static Vector2 CanvasSize { 
        get 
        {
            RectTransform canvasTr = GameObject.FindObjectOfType<Canvas>().GetComponent<RectTransform>();
            return new Vector2(canvasTr.rect.width, canvasTr.rect.height); 
        }
    }
    public static Vector2 ScreenToCanvas(Vector2 screenPos)
    {
        Vector2 canvasSize = CanvasSize;
        float x = screenPos.x / Screen.width * canvasSize.x;
        float y = screenPos.y / Screen.height * canvasSize.y;

        return new Vector2(x, y);
    }
}
