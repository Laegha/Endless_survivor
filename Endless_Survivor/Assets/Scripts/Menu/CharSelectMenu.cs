using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectMenu : MonoBehaviour
{
    [SerializeField] GameObject _mainMenu;
    [SerializeField] CharacterData[] _elegibleCharacters;
    [SerializeField] CharacterButton _selectCharBtnPrefab;
    [SerializeField] HorizontalLayoutGroup _gridRowPrefab;
    [SerializeField] RectTransform _buttonGrid;

    [SerializeField] RectTransform _gridMax;
    [SerializeField] RectTransform _gridMin;

    public void OnEnable()
    {
        var currentRow = Instantiate(_gridRowPrefab, _buttonGrid);
        float rowFilledSpace = 0;
        foreach (CharacterData character in _elegibleCharacters)
        {
            var characterButton = Instantiate(_selectCharBtnPrefab, currentRow.transform);
            characterButton.buttonImage.material = new Material(characterButton.buttonImage.material);
            characterButton.buttonImage.material.SetFloat("_MaxHeight", _gridMax.anchoredPosition.y);
            characterButton.buttonImage.material.SetFloat("_MinHeight", _gridMin.anchoredPosition.y);
            //characterButton.buttonImage.sprite = character.MenuImage;
            characterButton.characterData = character;
            characterButton.charSelectMenu = this;
            characterButton.SetImage(character.MenuImage);
            rowFilledSpace += _selectCharBtnPrefab.imageTargetSize.sizeDelta.x + currentRow.spacing;
            if(rowFilledSpace + _selectCharBtnPrefab.imageTargetSize.sizeDelta.x > _buttonGrid.sizeDelta.x)
            {
                currentRow = Instantiate(_gridRowPrefab, _buttonGrid);
                rowFilledSpace = 0;
            }
        }
    }

    public void CloseMenu()
    {
        Transform[] buttons = _buttonGrid.GetComponentsInChildren<Transform>();
        foreach (Transform button in buttons)
        {
            if (button != _buttonGrid)
                Destroy(button.gameObject);
        }

        _mainMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
