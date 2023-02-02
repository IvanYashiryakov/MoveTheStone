using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    private Level _level;

    private SerializedProperty _boardWidth;
    private SerializedProperty _boardHeight;
    private SerializedProperty _tryCount;
    //private SerializedProperty _items;

    private void OnEnable()
    {
        _level = (Level)target;
        if (_level.Items == null)
            _level.Items = new ItemInfo[0];

        _boardWidth = serializedObject.FindProperty("_boardWidth");
        _boardHeight = serializedObject.FindProperty("_boardHeight");
        _tryCount = serializedObject.FindProperty("_tryCount");
        //_items = serializedObject.FindProperty("_items");
    }

    public override void OnInspectorGUI()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.PropertyField(_boardWidth);
        EditorGUILayout.PropertyField(_boardHeight);
        GUILayout.EndHorizontal();
        EditorGUILayout.PropertyField(_tryCount);
        EditorGUILayout.Space(15);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Boxes", GUILayout.Width(100));

        if (GUILayout.Button("Add box", GUILayout.Width(70), GUILayout.Height(20)))
        {
            _level.Items = AddElementToArray(_level.Items);
        }

        GUILayout.EndHorizontal();

        GUILayout.BeginVertical(GUI.skin.box);
        for (int i = 0; i < _level.Items.Length; i++)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("#" + (i + 1).ToString(), GUILayout.Width(30));
            BoxType id = (BoxType)EditorGUILayout.EnumPopup(_level.Items[i].Id, GUILayout.Width(100));
            EditorGUILayout.LabelField("    X:", GUILayout.Width(26));
            int x = EditorGUILayout.IntField(_level.Items[i].X, GUILayout.Width(40));
            EditorGUILayout.LabelField("Y:", GUILayout.Width(14));
            int y = EditorGUILayout.IntField(_level.Items[i].Y, GUILayout.Width(40));
            EditorGUILayout.Space(5);

            ItemInfo itemInfo = new ItemInfo(id, x, y);
            _level.Items[i] = itemInfo;

            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                _level.Items = DeleteElementFromArray(_level.Items, i);
                i--;
            }

            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_level);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private ItemInfo[] AddElementToArray(ItemInfo[] array)
    {
        ItemInfo[] newArray = new ItemInfo[array.Length + 1];
        array.CopyTo(newArray, 0);

        if (array.Length > 0)
            newArray[array.Length] = newArray[array.Length - 1];

        return newArray;
    }

    private ItemInfo[] DeleteElementFromArray(ItemInfo[] array, int index)
    {
        ItemInfo[] newArray = new ItemInfo[array.Length - 1];
        int n = 0;

        for (int i = 0; i < array.Length; i++)
        {
            if (i != index)
            {
                newArray[n] = array[i];
                n++;
            }
        }

        return newArray;
    }
}