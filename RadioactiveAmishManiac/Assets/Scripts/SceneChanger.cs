using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{

    public static SceneChanger instance = null;

    public Transform image;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(this.gameObject);

        if (instance == this)
            DontDestroyOnLoad(this.gameObject);
    }

    public static void ChangeToScene(string sceneName)
    {
        if (instance == null)
            return;
        instance._ChangeScene(sceneName);
    }

    public void _ChangeScene(string sceneName)
    {
        StopAllCoroutines();
        StartCoroutine(DoChangeScene(sceneName));
    }

    IEnumerator DoChangeScene(string sceneName)
    {
        const float spinSpeed = 360.0f*3.0f;

        const float zoomTime = 0.7f;

        // zoom in
        image.gameObject.SetActive(true);
        for (float t = 0.0f; t < zoomTime; t += Time.unscaledDeltaTime)
        {
            image.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, EaseIn(t / zoomTime));
            image.Rotate(0.0f, 0.0f, spinSpeed * Time.unscaledDeltaTime);

            yield return new WaitForEndOfFrame();
        }

        image.localScale = Vector3.one;
        // start loading scene
        var s = SceneManager.LoadSceneAsync(sceneName);
        // wait for scene load
        while (!s.isDone)
        {
            image.Rotate(0.0f, 0.0f, spinSpeed * Time.unscaledDeltaTime);

            yield return new WaitForEndOfFrame();
        }

        // zoom out
        for (float t = 0.0f; t < zoomTime; t += Time.unscaledDeltaTime)
        {
            image.localScale = Vector3.Lerp(Vector3.one, Vector3.zero, EaseOut(t / zoomTime));
            image.Rotate(0.0f, 0.0f, spinSpeed * Time.unscaledDeltaTime);

            yield return new WaitForEndOfFrame();
        }
        image.gameObject.SetActive(true);
    }

    float EaseIn(float t)
    {
        return t * t * t;
    }

    float EaseOut(float t)
    {
        float t1 = t - 1.0f;
        return t1 * t1 * t1 + 1.0f;
    }

}
