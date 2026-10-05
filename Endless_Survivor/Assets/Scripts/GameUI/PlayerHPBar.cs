using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHPBar : MonoBehaviour
{
    [SerializeField] FilledSlider _hpSlider;
    [SerializeField] TextMeshProUGUI _hpText;
    public void SetHP(int remainingHP, int maxHP)
    {
        int clampedRemainingHp = (int)Mathf.Clamp(remainingHP, 0, Mathf.Infinity);
        _hpSlider.SetValue(clampedRemainingHp, maxHP);
        _hpText.text = clampedRemainingHp + "/" + maxHP;
    }
}
