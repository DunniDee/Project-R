using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Script_SceneManager))]
public class Debug_SceneManager : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        // DrawDefaultInspector();

        Script_SceneManager SM = (Script_SceneManager)target;
        if (GUILayout.Button("Dark"))
        {
           SM.FadeDark();
        }

        if (GUILayout.Button("Light"))
        {
           SM.FadeLight();
        }
    }
}
