﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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
    public Popup currentPopup;

    public Transform menu;
    public GameObject pauseMenu;
    public GameObject settingMenu;
    public GameObject gameUI;
    public GameObject mainMenu;
    public GameObject gameEnd;
    public Slider masterSound;
    public Slider bgmsound;
    public Slider sfxsound;

    private AudioSource mAudioSource;

    private void Awake()
    {
        currentState = States.MAINMENU;

        mAudioSource = GetComponent<AudioSource>();

        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(this.gameObject);

        if (menu != null)
        {
            pauseMenu.SetActive(false);
            settingMenu.SetActive(false);
            gameUI.SetActive(false);
            gameEnd.SetActive(false);
        }

        currentPopup = Popup.COUNT;
    }

    private void Update()
    {
        AudioListener.volume = masterSound.value;
        // Add in volume for background audio clips
        // Add in volume for regular sound clips

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
                Time.timeScale = 1.0f;
                SceneChanger.ChangeToScene("MainMenu");

                break;
            case States.GAME:
                Time.timeScale = 1.0f;
                SceneChanger.ChangeToScene("CamScene");
                gameUI.SetActive(true);

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
                gameEnd.SetActive(false);
                //mAudioSource.PlayOneShot();
                break;
            case States.GAME:
                gameUI.SetActive(true);
                pauseMenu.SetActive(false);
                settingMenu.SetActive(false);
                mainMenu.SetActive(false);
                gameEnd.SetActive(false);
                // mAudioSource.PlayOneShot();
                break;
        }
    }
}
