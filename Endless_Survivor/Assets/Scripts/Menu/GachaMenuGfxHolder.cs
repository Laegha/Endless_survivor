using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class GachaMenuGfxHolder
{
    public GameObject coinSlotOutline;
    public GameObject leverOutline;
    public Animator baseAnimator;
    public Animator faceAnimator;
    public Animator ballAnimator;
    public Animator ballPrizeAnimator;
    public ObjectRotatorByDrag leverRotator;
    public GameObject prizeBG;
    public Animator prizeInfoAnimator;
    public Image prizeImage;
    public RectTransform prizeImageTarget;
    public TextMeshProUGUI prizeTypeDisplay;
    public TextMeshProUGUI prizeNameDisplay;


}
