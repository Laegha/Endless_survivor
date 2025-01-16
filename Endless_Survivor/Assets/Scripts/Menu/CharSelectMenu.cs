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

    public void GenerateButtons()
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
        _mainMenu.SetActive(true);
        gameObject.SetActive(false);

        GameObject[] buttons = _buttonGrid.GetComponentsInChildren<GameObject>();
        foreach (GameObject button in buttons)
        {
            Destroy(button);
        }
    }
}
