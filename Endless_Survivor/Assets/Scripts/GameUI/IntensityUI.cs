using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntensityUI : MonoBehaviour
{
    [SerializeField] FilledSlider _levelProgressSlider;
    [SerializeField] Animator _engineAnimator;

    public void ChangeUI(float currProgress, float maxProgress, float animSpeedIncrease)
    {
        _levelProgressSlider.SetValue(currProgress, maxProgress);
        _engineAnimator.speed += animSpeedIncrease;
    }
}
