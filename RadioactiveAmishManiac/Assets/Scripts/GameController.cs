using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{

    public bool isPlaying = false;
    public Vector3 cameraDirection = Vector3.forward;

    [Header("Chunk Control")]
    public Transform farBoundary;
    public Transform deleteBoundary;

    Camera cam;
    LevelGenerator generator;

    private void Awake()
    {
        cam = Camera.main;
        generator = FindObjectOfType<LevelGenerator>();
        Assert.IsNotNull(generator, "LevelGenerator object must be in the scene!");
    }

    private void FixedUpdate()
    {
        if (isPlaying)
            UpdatePlaying();
    }

    // called every frame that the game is being played - not paused
    void UpdatePlaying()
    {
        generator.transform.position -= cameraDirection * Time.fixedDeltaTime;

        // make sure chunks are always generated at a specific position
        if (farBoundary)
            generator.PlaceUntil(farBoundary.position);
        // delete chunks that are beyond a certain point
        if (deleteBoundary)
            generator.DeleteBehind(deleteBoundary.position);
    }

}
