using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class IntensityUI : MonoBehaviour
{
    [SerializeField] FilledSlider _levelProgressSlider;
    [SerializeField] Animator _engineAnimator;
    [SerializeField] string _intensityLevelText;
    [SerializeField] TextMeshProUGUI _intensityLevelDisplay;

    public void ChangeUI(float currProgress, float maxProgress, float animSpeedIncrease)
    {
        _levelProgressSlider.SetValue(currProgress, maxProgress);
        _engineAnimator.speed += animSpeedIncrease;
        _intensityLevelDisplay.text = _intensityLevelText + IntensityManager.im.CurrIntensityLevel;
    }
}
