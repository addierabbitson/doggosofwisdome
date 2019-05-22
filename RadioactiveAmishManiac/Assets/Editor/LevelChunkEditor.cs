using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelChunk))]
public class LevelChunkEditor : Editor
{

    public void OnSceneGUI()
    {
        LevelChunk chunk = target as LevelChunk;
        if (chunk == null)
            return;

        Handles.DrawWireCube(chunk.transform.position, chunk.bounds);
    }

}
