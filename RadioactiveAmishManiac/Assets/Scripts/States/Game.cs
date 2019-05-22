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
        gameManager.pauseMenu.SetActive(true);
    }

    public void Replay()
    {
        SceneManager.LoadScene("Gabe");

        BaseState[] b1 = gameManager.gameObject.GetComponents<BaseState>();
        foreach (var s in b1)
            s.enabled = false;

        Game scene = gameManager.gameObject.GetComponent<Game>();
        scene.enabled = true;

        gameManager.gameEnd.SetActive(false);
    }

    public void Menu()
    {
         
    }
}