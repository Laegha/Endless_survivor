using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuObj;
    [SerializeField] CharacterButton _selectCharBtnPrefab;
    [SerializeField] HorizontalLayoutGroup _gridRowPrefab;
    [SerializeField] RectTransform _buttonGrid;

    public void DisplayMenu()
    {
        _menuObj.SetActive(true);
        DisplayCharacters();
    }
    async void DisplayCharacters()
    {
        var currentRow = Instantiate(_gridRowPrefab, _buttonGrid);
        float rowFilledSpace = 0;
        var unlockedCharacters = await UnlockmentsManager.UnlockedCharacters();
        foreach (CharacterData character in unlockedCharacters)
        {
            var characterButton = Instantiate(_selectCharBtnPrefab, currentRow.transform);
            //characterButton.buttonImage.sprite = character.MenuImage;
            characterButton.characterData = character;
            characterButton.charSelectMenu = this;
            characterButton.SetImage(character.MenuImage);
            rowFilledSpace += _selectCharBtnPrefab.imageTargetSize.sizeDelta.x + currentRow.spacing;
            if (rowFilledSpace + _selectCharBtnPrefab.imageTargetSize.sizeDelta.x > _buttonGrid.sizeDelta.x)
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
        _menuObj.SetActive(false);
    }
}
