using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(PlatformController))]
public class PlatformEditor : Editor
{

    private void OnSceneGUI()
    {
        PlatformController p = target as PlatformController;

        p.position1 = Handles.PositionHandle(p.position1 + p.transform.position, Quaternion.identity) - p.transform.position;
        p.position2 = Handles.PositionHandle(p.position2 + p.transform.position, Quaternion.identity) - p.transform.position;
    }

}
