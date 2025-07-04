using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] GameObject _menuObject;

    private void Start()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void ChangeMenu(GameObject menu)
    {
        menu.SetActive(true);
        _menuObject.SetActive(false);
    }
}
