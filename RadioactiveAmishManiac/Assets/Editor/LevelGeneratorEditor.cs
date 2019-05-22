using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(LevelGenerator))]
public class LevelGeneratorEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        LevelGenerator gen = target as LevelGenerator;

        if (GUILayout.Button("Generate"))
            gen.PlaceChunk();

        if (GUILayout.Button("Clear Chunks"))
            gen.ClearChunks();
    }

}
