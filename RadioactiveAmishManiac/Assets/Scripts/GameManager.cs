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
                SceneManager.LoadScene("Main");

                BaseState[] b1 = GetComponents<BaseState>();
                foreach (var s in b1)
                    s.enabled = false;

                Game scene1 = GetComponent<Game>();
                scene1.enabled = true;
                break;
        }
    }
}
