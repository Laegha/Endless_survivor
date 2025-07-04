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

    public void SelectCharacter()
    {
        GameManager.gm.selectedCharacter = characterData;
        charSelectMenu.CloseMenu();
    }

    public void SetImage(Sprite imageSprite)
    {
        buttonImage.sprite = imageSprite;
        Utility.ScaleImageToFitTarget(buttonImage.GetComponent<RectTransform>(), imageSprite, imageTargetSize.sizeDelta);
    }
}
