using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelGenerator : MonoBehaviour
{

    [Header("Chunks to choose from for the start")]
    public List<LevelChunk> startChunks;
    [Header("Chunks for rest of the level")]
    public List<LevelChunk> levelChunks;
    public Vector3 placeDirection = Vector3.forward;

    Vector3 nextPlace;
    int lastChunkIndex = -1;
    List<LevelChunk> placedChunks = new List<LevelChunk>();

    public void PlaceChunk()
    {
        LevelChunk toSpawn = placedChunks.Count > 0 ? GetRandomLevelChunk() : GetRandomStartChunk();
        if(toSpawn == null)
        {
            Debug.Log(lastChunkIndex);
            return;
        }
        // make new chunk
        LevelChunk newChunk = Instantiate(toSpawn, transform);

        Vector3 directionCheck = placeDirection;
        directionCheck.x *= newChunk.bounds.x;
        directionCheck.y *= newChunk.bounds.y;
        directionCheck.z *= newChunk.bounds.z;
        float directionLength = directionCheck.magnitude;

        if (placedChunks.Count > 0)
            nextPlace += directionLength * placeDirection * 0.5f;
        // place it properly
        newChunk.transform.localPosition = nextPlace;

        nextPlace += directionLength * placeDirection * 0.5f;

        placedChunks.Add(newChunk);
    }

    public void PlaceUntil(Vector3 pos)
    {
        float dot = Vector3.Dot(pos - (transform.position + nextPlace), placeDirection);
        if (dot < 0)
            return;

        float placeDist = float.MaxValue;
        float oldPlaceDist = float.MaxValue;
        int placeCount = 0;
        do
        {
            oldPlaceDist = placeDist;

            PlaceChunk();

            placeDist = Vector3.Distance(pos, transform.position + nextPlace);
            placeCount++;
            if (placeCount > 1000)
                break;
        } while (placeDist < oldPlaceDist);
    }

    public void DeleteBehind(Vector3 pos)
    {
        for (int i = placedChunks.Count - 1; i >= 0; --i)
        {
            LevelChunk c = placedChunks[i];

            float dot = Vector3.Dot(pos - c.transform.position, -1.0f * placeDirection);
            if (dot > 0)
                continue;

            Destroy(c.gameObject);
            placedChunks.RemoveAt(i);
        }
    }

    public void ClearChunks()
    {
        foreach (var c in placedChunks)
        {
            if (c == null || c.gameObject == null)
                continue;
            if (Application.isEditor)
                DestroyImmediate(c.gameObject);
            else
                Destroy(c.gameObject);
        }
        placedChunks.Clear();

        nextPlace = Vector3.zero;
    }

    LevelChunk GetRandomLevelChunk()
    {
        // choose random chunk index that wasn't chosen last time
        int chunkIndex = 0;
        do
        {
            chunkIndex = Random.Range(0, levelChunks.Count);
        } while (chunkIndex == lastChunkIndex);
        // keep track of this index so we don't spawn the same one twice
        lastChunkIndex = chunkIndex;

        return levelChunks[chunkIndex];
    }

    LevelChunk GetRandomStartChunk()
    {
        if (startChunks.Count <= 0)
            return GetRandomLevelChunk();
        return startChunks[Random.Range(0, startChunks.Count)];
    }

}
