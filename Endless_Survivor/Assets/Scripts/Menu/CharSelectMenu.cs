using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharSelectMenu : MonoBehaviour
{
    [SerializeField] CharacterData[] _elegibleCharacters;
    [SerializeField] GameObject _selectCharBtnPrefab;
    [SerializeField] Transform _buttonGrid;
    [SerializeField] GameObject _mainMenu;

    public void OnEnable()
    {
        foreach (CharacterData character in _elegibleCharacters)
        {
            GameObject button = Instantiate(_selectCharBtnPrefab, _buttonGrid);
            button.GetComponent<Image>().sprite = character.MenuImage;
            CharacterButton characterButton = button.GetComponent<CharacterButton>();
            characterButton.characterData = character;
            characterButton.charSelectMenu = this;
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
