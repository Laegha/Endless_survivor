using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScalableToTargetImage : MonoBehaviour
{
    [SerializeField] Image _image;
    [SerializeField] RectTransform _target;

    public void ChangeImageSprite(Sprite newSprite)
    {
        Utility.ScaleImageToFitTarget(_image.GetComponent<RectTransform>(), newSprite, _target.sizeDelta);
        _image.sprite = newSprite;
    }
}
