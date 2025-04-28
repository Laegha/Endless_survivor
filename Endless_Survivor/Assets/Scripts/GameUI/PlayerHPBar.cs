using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] Slider _hpSlider;
    [SerializeField] TextMeshProUGUI _hpText;
    public void SetHP(int remainingHP, int maxHP)
    {
        float sliderValue = Mathf.InverseLerp(0, maxHP, remainingHP);
        _hpSlider.value = sliderValue;
        _hpText.text = remainingHP + "/" + maxHP;
    }
}
