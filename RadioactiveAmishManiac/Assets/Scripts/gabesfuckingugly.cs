using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gabesfuckingugly : MonoBehaviour
{
    public static gabesfuckingugly Instance;

    private void Awake()
    {
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
}
