using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : BaseState
{
    public void StartButton()
    {
        gameManager.currentState = States.GAME;
        gameManager.currentPopup = Popup.COUNT;
    }
}
