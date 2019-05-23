using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/*
 * hello
 * please put this on an empty gameobject which has the platform object
 * as a child! thank u
 */

public class PlatformController : MonoBehaviour
{

    public Vector3 position1;
    public Vector3 position2;
    public float moveInterval = 1.0f;
    public bool moveSmooth = true;

    float timer = 0.0f;
    Transform toMove = null;

    private void Awake()
    {
        timer = Random.Range(0.0f, moveInterval * 2.0f);
        foreach (Transform t in transform)
            toMove = t;

        Assert.IsNotNull(toMove, "This script should be placed on an empty gameobject with the platform's object as a child of it!");
    }

    private void FixedUpdate()
    {
        if (toMove == null)
            return;

        timer += Time.fixedDeltaTime;

        float t = 0.0f;
        if (moveSmooth)
        {
            t = (Mathf.Sin(timer / moveInterval) * 0.5f) + 0.5f;
        }
        else
        {
            t = (timer / moveInterval) % 2.0f;
            if (t > 1.0f)
            {
                t -= 1.0f;
                t = 1.0f - t;
            }
        }
        toMove.localPosition = Vector3.Lerp(position1, position2, t);
    }


}
