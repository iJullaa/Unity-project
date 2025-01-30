﻿using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MenuController))]
public class MenuEditor : Editor {

    int selectedOption = 0;
    private MenuController menu;

    public override void OnInspectorGUI()
    {
        menu = target as MenuController;
        //base.OnInspectorGUI();
        string[] labels = new string[] {"Parallax Backgrounds"};
        selectedOption = GUILayout.SelectionGrid(selectedOption, labels, 1);
        switch (selectedOption)
        {
            case 0:
                menu.useParallax = true;
                menu.useKeys = EditorGUILayout.Toggle("Use keyboard keys", menu.useKeys);
                EditorGUILayout.HelpBox("If deactivated the menu will only use the ingame arrows.", MessageType.Info);
                serializedObject.Update();
                EditorGUILayout.PropertyField(serializedObject.FindProperty("mainBackgroundParallax"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("backgroundsParallax"), true);            
                EditorGUILayout.PropertyField(serializedObject.FindProperty("options"), true);
                EditorGUILayout.PropertyField(serializedObject.FindProperty("About"), true);
                EditorGUILayout.HelpBox("The audio that will be played in the menu.", MessageType.Info);
                serializedObject.ApplyModifiedProperties();
                break;

        }
    }


}



