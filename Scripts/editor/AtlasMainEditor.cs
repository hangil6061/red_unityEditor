using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(AtlasMain))]
public class AtlasMainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        AtlasMain main = target as AtlasMain;

        if (GUILayout.Button("Save atlas"))
        {
            main.SaveAtlas();
        }
    }
}
