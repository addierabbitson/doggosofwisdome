using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    public static CameraScript instance;

    Vector3 originalPos;

    float shakeTime = 0.0f;
    float shakeIntensity = 1.0f;

    private void Awake()
    {
        instance = this;

        originalPos = transform.position;
    }

    private void FixedUpdate()
    {
        if (shakeTime <= 0.0f)
            return;

        shakeTime -= Time.fixedDeltaTime;
        float shakeAmount = Mathf.Min(shakeTime,1.0f) * shakeIntensity;

        Vector3 offset = Vector3.zero;

        offset += transform.right * Random.Range(-shakeAmount, shakeAmount);
        offset += transform.up * Random.Range(-shakeAmount, shakeAmount);

        transform.position = originalPos + offset;
    }

    public void ShakeCamera(float length, float intensity = 1.0f)
    {
        shakeTime = length;
        shakeIntensity = intensity;
    }

}
