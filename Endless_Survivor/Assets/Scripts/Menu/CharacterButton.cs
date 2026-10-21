using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterButton : MonoBehaviour
{
    [HideInInspector] public CharSelectMenu charSelectMenu;
    [HideInInspector] public CharacterData characterData;
    public Image buttonImage;
    public RectTransform imageTargetSize;
    [SerializeField] Animator _charSelectedAnimator;

    public void SelectCharacter()
    {
        GameManager.gm.SelectedCharacter = characterData;
        UnlockmentsManager.SetNotNewCharacter(characterData);
        //charSelectMenu.CloseMenu();
        charSelectMenu.UpdateAllSelections();
    }

    public void SetImage(Sprite imageSprite)
    {
        buttonImage.sprite = imageSprite;
        Utility.ScaleImageToFitTarget(buttonImage.GetComponent<RectTransform>(), imageSprite, imageTargetSize.sizeDelta);
        UpdateSelection();
    }
    public void UpdateSelection()
    {
        _charSelectedAnimator.Play(GameManager.gm.SelectedCharacter == characterData ? "Selected" : "Unselected");
    }
}
