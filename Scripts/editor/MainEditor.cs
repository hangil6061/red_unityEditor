using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Main))]
public class MainEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        Main main = target as Main;

        if( GUILayout.Button("Save root") )
        {
            main.SaveScene();
        }

        if (GUILayout.Button("Save atlas"))
        {
            main.SaveAtlas();
        }

        if (GUILayout.Button("Save AnimClip"))
        {
            main.SaveAnimClip();
        }
    }
}
