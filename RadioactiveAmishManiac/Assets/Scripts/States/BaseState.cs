using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Popup
{
    GAMEEND,
    PAUSE,

    COUNT
}

public class BaseState : MonoBehaviour
{
    public GameManager gameManager;

    private void Start()
    {
        gameManager = GameManager.Instance;
    }

}
