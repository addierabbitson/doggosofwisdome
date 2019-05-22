using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum States
{
    MAINMENU,
    GAME,

    COUNT
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public States currentState;
    public States previousState;

    public Transform menu;
    public GameObject pauseMenu;
    public GameObject settingMenu;
    public GameObject gameUI;
    public GameObject mainMenu;
    public GameObject gameEnd;

    private void Awake()
    {
        currentState = States.MAINMENU;

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        if (menu == null)
        {
            menu = GameObject.Find("Canvas").transform;
            pauseMenu = GameObject.Find("PauseMenu").gameObject;
            settingMenu = GameObject.Find("SettingsMenu").gameObject;
            gameUI = GameObject.Find("GUI").gameObject;
            mainMenu = GameObject.Find("MainMenu").gameObject;
            gameEnd = GameObject.Find("GameEnd").gameObject;
            pauseMenu.SetActive(false);
            settingMenu.SetActive(false);
            gameUI.SetActive(false);
            gameEnd.SetActive(false);
        }
        DontDestroyOnLoad(menu.gameObject);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha0))
            currentState = States.MAINMENU;
        if (Input.GetKeyDown(KeyCode.Alpha1))
            currentState = States.GAME;

        if (currentState == previousState)
            return;

        previousState = currentState;
        SceneManager.sceneLoaded += OnSceneLoad;

        switch (currentState)
        {
            case States.MAINMENU:
                SceneManager.LoadScene("MainMenu");

                BaseState[] b = GetComponents<BaseState>();
                foreach (var s in b)
                    s.enabled = false;

                MainMenu scene = GetComponent<MainMenu>();
                scene.enabled = true;
                break;
            case States.GAME:
                SceneManager.LoadScene("Gabe");

                BaseState[] b1 = GetComponents<BaseState>();
                foreach (var s in b1)
                    s.enabled = false;

                Game scene1 = GetComponent<Game>();
                scene1.enabled = true;
                break;
        }
    }

    public void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        switch (currentState)
        {
            case States.MAINMENU:
                mainMenu.SetActive(true);
                pauseMenu.SetActive(false);
                settingMenu.SetActive(false);
                gameUI.SetActive(false);
                break;
            case States.GAME:
                gameUI.SetActive(true);
                pauseMenu.SetActive(false);
                settingMenu.SetActive(false);
                mainMenu.SetActive(false);
                break;
        }
    }
}
