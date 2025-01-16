using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject charSelectMenu;

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenCharSelect()
    {
        charSelectMenu.SetActive(true);
        gameObject.SetActive(false);
    }
}
