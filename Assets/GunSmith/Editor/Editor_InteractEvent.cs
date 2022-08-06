using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(Script_InteractEvent))]
[CanEditMultipleObjects]
public class Editor_InteractEvent : Editor
{
    Script_InteractEvent targetScript;
    SerializedProperty InteractEvent;
    SerializedProperty InteractType;

    SerializedProperty ColliderCenter;
    SerializedProperty ColliderSize;
    SerializedProperty ColliderColor;
    SerializedProperty DoEventOnce;
    SerializedProperty IsActive;


    public Object TargetObject;

    // OnEnable is called when the object is loaded.
    private void OnEnable()
    {
        targetScript = (Script_InteractEvent)target;
        InteractEvent = serializedObject.FindProperty("InteractEvent");
        InteractType = serializedObject.FindProperty("EventType");
        ColliderCenter = serializedObject.FindProperty("TriggerCenter");
        ColliderSize = serializedObject.FindProperty("TriggerSize");
        DoEventOnce = serializedObject.FindProperty("DoEventOnce");
        IsActive = serializedObject.FindProperty("IsActive");
        ColliderColor = serializedObject.FindProperty("TriggerColor");

    }

    void DrawInteractButton()
    {
        //GUI.enabled = targetScript.EventType == Script_InteractEvent.InteractEventType.Interact;
        
        if (GUILayout.Button("Interact"))
        {
            targetScript.Interact();
        }
        GUI.enabled = true;
    }

    void DrawTriggerBoxProperties()
    {
        GUI.enabled = targetScript.EventType == Script_InteractEvent.InteractEventType.ColliderEnter || 
        targetScript.EventType == Script_InteractEvent.InteractEventType.ColliderExit;
        GUILayout.Label("Trigger Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(ColliderCenter);
        EditorGUILayout.PropertyField(ColliderSize);
        EditorGUILayout.PropertyField(ColliderColor);
        GUI.enabled = true;
    }

    void DrawEventProperties()
    {
        GUILayout.Label("Event Properties", EditorStyles.boldLabel);
        EditorGUILayout.PropertyField(DoEventOnce, new GUIContent("Do Event Once"));
        EditorGUILayout.PropertyField(IsActive, new GUIContent("Is Event Active"));
        GUILayout.Space(10);
    }
    // Draw the GUI for the inspector
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(InteractType, new GUIContent("Interact EventType"));
        EditorGUILayout.PropertyField(InteractEvent, new GUIContent("Interact Events"));

        DrawEventProperties();
        DrawTriggerBoxProperties();
        DrawInteractButton();
       
        serializedObject.ApplyModifiedProperties();
    }
   
    // Update is called once per frame
    void Update()
    {
        

    }
}
