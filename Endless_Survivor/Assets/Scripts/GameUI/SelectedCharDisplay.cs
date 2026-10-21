using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class SelectedCharDisplay : MonoBehaviour
{
    [SerializeField] ScalableToTargetImage _charImage;
    [SerializeField] Animator _selectorAnimator;
    [SerializeField] TextMeshProUGUI _charNameDisplay;
    void Start()
    {
        GameManager.gm.OnSelectedCharChange += ChangeCharacter;
        if(GameManager.gm.SelectedCharacter != null)
            ChangeCharacter(GameManager.gm.SelectedCharacter);
    }
    private void OnEnable()
    {
        _selectorAnimator.Play("Selected");
    }
    void ChangeCharacter(CharacterData selectedChar)
    {
        _charImage.ChangeImageSprite(selectedChar.MenuImage);
        _charNameDisplay.text = selectedChar.CharacterName;
    }
    private void OnDestroy()
    {
        Debug.Log("DESTROYED THE PLAYER THING");
        GameManager.gm.OnSelectedCharChange -= ChangeCharacter;
    }
}
