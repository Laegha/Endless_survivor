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

    public static T GetRouletteElement<T>(List<RouletteElementChance<T>> selectableElements)
    {
        Dictionary<RouletteElementKey<T>, int> rouletteElements = new Dictionary<RouletteElementKey<T>, int>();// WE USE RouletteElementKey IN CASE RouletteElementChance.element is null, therefore it can't be added as a dict key
        //PickupDataKey nullPickupData = new PickupDataKey(null);
        //possiblePickups.Add(nullPickupData, 100);
        foreach (var element in selectableElements)
        {
            //possiblePickups[nullPickupData] = Mathf.Clamp(possiblePickups[nullPickupData] - dropablePickupChance.Chance, 0, 100);
            rouletteElements.Add(new(element.Element), element.Chance);
        }
        Roulette<RouletteElementKey<T>> roulette = new Roulette<RouletteElementKey<T>>(rouletteElements);
        T resultElement = roulette.Spin().element;
        return resultElement;
    }
    public static T GetRouletteElementWithNullChance<T>(List<RouletteElementChance<T>> selectableElements) where T : class
    {
        List<RouletteElementChance<T>> possibleElements = new List<RouletteElementChance<T>>();
        int nullChance = 100;
        foreach (var dropablePickup in selectableElements)
        {
            nullChance -= dropablePickup.Chance;
            possibleElements.Add(dropablePickup);

        }
        possibleElements.Add(new(null, nullChance));

        var resultPickup = GetRouletteElement(possibleElements);
        return resultPickup;
    }
    public static PickupControl GeneratePickup(PickupData generatedPickup, Vector2 pickupPosition)
    {
        PickupControl newPickup = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["Pickup"], pickupPosition, Quaternion.identity).GetComponent<PickupControl>();
        generatedPickup.TransferData(newPickup);
        return newPickup;
    }
    public static SupportObjectControl GenerateSupportObj(SupportObjectData generatedSupportObjectData, Vector2 supportObjPosition, Quaternion supportObjRotation)
    {
        SupportObjectControl supportObj = GameObject.Instantiate(GameManager.gm.prefabHolder.Prefabs["SupportObject"], supportObjPosition, supportObjRotation).GetComponent<SupportObjectControl>();
        generatedSupportObjectData.TransferData(supportObj);
        return supportObj;
    }
    public static Vector2 GetRandomPosInMap()
    {
        return Vector2.zero;
    }
    public static Vector2 GetPerpendicularVector(Vector2 vector)
    {
        float perpendicularX = 1;
        float perpendicularY = (-(perpendicularX * vector.x)) / vector.y;
        return new Vector2(perpendicularX, perpendicularY).normalized;
    }
}
