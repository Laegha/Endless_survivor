using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuObj;
    [SerializeField] GameObject _mainMenuObj;
    [SerializeField] CharacterButton _selectCharBtnPrefab;
    [SerializeField] GameObject _newCharIndicatorPrefab;
    [SerializeField] HorizontalLayoutGroup _gridRowPrefab;
    [SerializeField] RectTransform _buttonGridTr;
    [SerializeField] RectTransform _returnButton;
    VerticalLayoutGroup _buttonGrid;

    private void Start()
    {
        _buttonGrid = _buttonGridTr.GetComponent<VerticalLayoutGroup>();
    }

    public void DisplayMenu()
    {
        _menuObj.SetActive(true);
        DisplayCharacters();
    }
    async void DisplayCharacters()
    {
        var currentRow = Instantiate(_gridRowPrefab, _buttonGridTr);
        int gridRows = 1;
        float rowFilledSpace = 0;
        var unlockedCharacters = await UnlockmentsManager.UnlockedCharacters();
        foreach (var character in unlockedCharacters)
        {
            var characterButton = Instantiate(_selectCharBtnPrefab, currentRow.transform);
            //characterButton.buttonImage.sprite = character.MenuImage;
            characterButton.characterData = character.element;
            characterButton.charSelectMenu = this;
            characterButton.SetImage(character.element.MenuImage);
            if(character.isNew)
                Instantiate(_newCharIndicatorPrefab, characterButton.imageTargetSize.transform);

            rowFilledSpace += _selectCharBtnPrefab.imageTargetSize.sizeDelta.x + currentRow.spacing;
            if (rowFilledSpace + _selectCharBtnPrefab.imageTargetSize.sizeDelta.x > _buttonGridTr.sizeDelta.x)
            {
                currentRow = Instantiate(_gridRowPrefab, _buttonGridTr);
                gridRows ++;
                rowFilledSpace = 0;
            }
        }
        float rowFilledSize = (_buttonGrid.spacing + _gridRowPrefab.GetComponent<RectTransform>().sizeDelta.y) * gridRows;
        _buttonGridTr.sizeDelta = Vector2.right * _buttonGridTr.sizeDelta + Vector2.up * (rowFilledSize + _buttonGrid.padding.top + _returnButton.sizeDelta.y);


    }

    public void CloseMenu()
    {
        Transform[] buttons = _buttonGridTr.GetComponentsInChildren<Transform>();
        foreach (Transform button in buttons)
        {
            if (button != _buttonGridTr)
                Destroy(button.gameObject);
        }
        _menuObj.SetActive(false);
        _mainMenuObj.SetActive(true);
    }
}
