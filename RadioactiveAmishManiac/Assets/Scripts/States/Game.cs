using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : BaseState
{
    private void Update()
    {

    }

    public void PauseGame()
    {
        gameManager.pauseMenu.SetActive(true);
        gameManager.gameUI.SetActive(false);
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        gameManager.pauseMenu.SetActive(false);
        gameManager.gameUI.SetActive(true);
        Time.timeScale = 1.0f;
    }

    public void SettingsMenu()
    {
        gameManager.pauseMenu.SetActive(false);
        gameManager.settingMenu.SetActive(true);
    }

    public void BackToPause()
    {
        gameManager.settingMenu.SetActive(false);
        if (SceneManager.GetActiveScene().name != "MainMenu")
            gameManager.pauseMenu.SetActive(true);
    }

    public void Twitter() {
        Application.OpenURL("https://twitter.com/fsh_zone");
    }

    public void Replay()
    {
        gameManager.gameEnd.SetActive(false);
        SceneManager.LoadScene("CamScene");
        Time.timeScale = 1.0f;
    }

    public void Menu()
    {
        gameManager.currentState = States.MAINMENU;
    }
}