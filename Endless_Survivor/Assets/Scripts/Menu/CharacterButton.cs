using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterButton : MonoBehaviour
{
    [HideInInspector] public CharSelectMenu charSelectMenu;
    [HideInInspector] public CharacterData characterData;

    public void SelectCharacter()
    {
        GameManager.gm.selectedCharacter = characterData;
        charSelectMenu.CloseMenu();
    }
}
