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
        _hpSlider.SetValue(remainingHP, maxHP);
        _hpText.text = remainingHP + "/" + maxHP;
    }
}
