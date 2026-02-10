using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FilledSlider : MonoBehaviour
{
    [SerializeField] Slider _fill;
    public void SetValue(float value, float max)
    {
        float sliderValue = Mathf.InverseLerp(0, max, value);
        _fill.value = sliderValue;
    }
}
