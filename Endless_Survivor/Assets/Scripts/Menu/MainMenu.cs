using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuObject;


    public void StartGame()
    {
        SceneManager.LoadScene("GameLoading");
    }

    public void ChangeMenu(GameObject menu)
    {
        menu.SetActive(true);
        _menuObject.SetActive(false);
    }
}
