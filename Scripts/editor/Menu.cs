using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public static class Menu
{
    [MenuItem("RED/Init/Main", false, 0)]
    public static void Main()
    {
        var main = Object.FindObjectOfType<Main>();
        if( main == null )
        {
            main = new GameObject("ToolMain").AddComponent<Main>();
        }

        if( main.scene == null )
        {
            var canvas = Object.FindObjectOfType<Canvas>();
            if( canvas == null )
            {
                canvas = new GameObject("Canvas").AddComponent<Canvas>();
                canvas.renderMode = RenderMode.ScreenSpaceOverlay;
            }

            main.scene = new GameObject("root");
            var scene = main.scene.AddComponent<RectTransform>();
            scene.SetParent(canvas.transform);
            scene.sizeDelta = Vector2.zero;
            scene.SetAnchor(AnchorPresets.TopLeft);
        }

        Selection.activeGameObject = main.scene;
    }

}


