using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class gabesfuckingugly : MonoBehaviour
{
    public static gabesfuckingugly Instance;

    public List<TextMeshProUGUI> scoreTexts;
    public List<TextMeshProUGUI> bestTexts;

    public AudioMixer mainMixer;
    public Slider volumeSlider;

    [Header("Tap to play wobble")]
    public Transform tapToPlayText;
    public float pulseSpeed = 2.0f;
    public float pulseAmount = 0.2f;
    public float rotateSpeed = 3.0f;
    public float rotateAmount = 45.0f;

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

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("MasterVolume", 0.8f);
        SetVolume(volumeSlider.value);
    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            float scaleTime = Time.timeSinceLevelLoad * pulseSpeed;
            tapToPlayText.localScale = Vector3.one + (Vector3.one * Mathf.Sin(scaleTime)*pulseAmount);

            float r = Mathf.Sin(Time.timeSinceLevelLoad * rotateSpeed) * rotateAmount;
            tapToPlayText.localRotation = Quaternion.Euler(0.0f, 0.0f, r);
        }
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

    public void SetVolume(float vol)
    {
        PlayerPrefs.SetFloat("MasterVolume", vol);
        mainMixer.SetFloat("MasterVolume", ConvertToDecibel(vol));
    }

    float ConvertToDecibel(float value)
    {
        return Mathf.Log10(Mathf.Max(value, 0.0001f)) * 20f;
    }
}
