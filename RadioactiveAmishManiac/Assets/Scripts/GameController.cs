using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class GameController : MonoBehaviour
{

    public bool isPlaying = false;
    public Vector3 cameraDirection = Vector3.forward;
    float scrollMultiplier = 1.0f;

    [Header("Chunk Control")]
    public Transform farBoundary;
    public Transform deleteBoundary;

    Camera cam;
    LevelGenerator generator;
    PlayerController player;

    private void Awake()
    {
        cam = Camera.main;
        generator = FindObjectOfType<LevelGenerator>();
        Assert.IsNotNull(generator, "LevelGenerator object must be in the scene!");
        player = FindObjectOfType<PlayerController>();
        Assert.IsNotNull(player, "PlayerController object must be in the scene!");
    }

    private void FixedUpdate()
    {
        if (isPlaying)
            UpdatePlaying();

        // calculate how far the player is from the origin
        Vector3 dif = generator.placeDirection;
        Vector3 plr = player.transform.position;
        dif.x = Mathf.Max(0.0f, plr.x * dif.x);
        dif.y = Mathf.Max(0.0f, plr.y * dif.y);
        dif.z = Mathf.Max(0.0f, plr.z * dif.z);
        float dist = dif.magnitude;

        if (dist > 2.0f)
            scrollMultiplier = dist - 1.0f;
        else
            scrollMultiplier = 1.0f;
    }

    // called every frame that the game is being played - not paused
    void UpdatePlaying()
    {
        generator.transform.position -= (cameraDirection * Time.fixedDeltaTime) * scrollMultiplier;

        // make sure chunks are always generated at a specific position
        if (farBoundary)
            generator.PlaceUntil(farBoundary.position);
        // delete chunks that are beyond a certain point
        if (deleteBoundary)
            generator.DeleteBehind(deleteBoundary.position);
    }

}
