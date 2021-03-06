﻿using System.Collections;
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
        gameManager.currentPopup = Popup.PAUSE;
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
        {
            if (gameManager.currentPopup == Popup.PAUSE)
                gameManager.pauseMenu.SetActive(true);
            else if(gameManager.currentPopup == Popup.GAMEEND)
                gameManager.gameEnd.SetActive(true);
        }
    }

    public void Twitter() {
        Application.OpenURL("https://twitter.com/fsh_zone");
    }

    public void Replay()
    {
        gameManager.gameEnd.SetActive(false);
        SceneChanger.ChangeToScene("CamScene");
        Time.timeScale = 1.0f;
    }

    public void MainMenu()
    {
        SceneChanger.ChangeToScene("MainMenu");
        gameManager.currentState = States.MAINMENU;
        gameManager.currentPopup = Popup.COUNT;
    }
}