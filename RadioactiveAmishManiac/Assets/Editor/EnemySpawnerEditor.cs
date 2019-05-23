using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(EnemySpawner))]
public class EnemySpawnerEditor : Editor
{

    private void OnSceneGUI()
    {
        EnemySpawner spawner = target as EnemySpawner;
        if (spawner == null)
            return;

        Handles.color = Color.cyan;
        Handles.Label(spawner.transform.position, "start point");

        spawner.endPoint = Handles.PositionHandle(spawner.endPoint + spawner.transform.position, Quaternion.identity) - spawner.transform.position;
        Handles.Label(spawner.endPoint + spawner.transform.position + Vector3.up*0.1f, "end point");
    }

}
