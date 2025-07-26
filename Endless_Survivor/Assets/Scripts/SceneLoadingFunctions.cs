using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoadingFunctions : MonoBehaviour
{
    static SceneLoadingFunctions instance;
    public static SceneLoadingFunctions slf {  get { return instance; } }
    private void Awake()
    {
        instance = this;
    }
    public void Menu()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
    public void Game()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }
}
