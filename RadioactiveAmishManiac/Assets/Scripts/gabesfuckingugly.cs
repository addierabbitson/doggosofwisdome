using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gabesfuckingugly : MonoBehaviour
{
    public static gabesfuckingugly Instance;

    public List<TextMeshProUGUI> scoreTexts;
    public List<TextMeshProUGUI> bestTexts;

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
        UpdateBestTexts();
    }

    public void SetScore(int score)
    {
        foreach (var t in scoreTexts)
            t.text = "SCORE: " + score.ToString();
        UpdateBestTexts();
    }

    void UpdateBestTexts()
    {
        int best = PlayerPrefs.GetInt("highscore", 0);
        foreach (var t in bestTexts)
            t.text = "BEST: " + best.ToString();
    }


}
