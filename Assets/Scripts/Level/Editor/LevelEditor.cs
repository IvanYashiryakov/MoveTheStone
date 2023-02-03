using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Level))]
public class LevelEditor : Editor
{
    [SerializeField] private Texture _package;
    [SerializeField] private Texture _redWood;
    [SerializeField] private Texture _wood;
    [SerializeField] private Texture _green;
    [SerializeField] private Texture _baggage;
    [SerializeField] private Texture _steel;

    private Texture[] _textures;
    private Level _level;

    private void OnEnable()
    {
        _level = (Level)target;

        if (_level.Items == null)
            _level.Items = new ItemInfo[0];

        if (_level.Hints == null)
            _level.Hints = new HintInfo[0];

        _textures = new Texture[] { _package, _redWood, _wood, _green, _baggage, _steel };
    }

    public override void OnInspectorGUI()
    {
        DrawIntSection();
        DrawItemInfoSection();
        DrawGridView();
        DrawHintSection();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_level);
        }

        serializedObject.ApplyModifiedProperties();
    }

    private void DrawIntSection()
    {
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Width:", GUILayout.Width(40));
        _level.BoardWidth = EditorGUILayout.IntField(_level.BoardWidth, GUILayout.Width(30));
        EditorGUILayout.LabelField("Height:", GUILayout.Width(45));
        _level.BoardHeight = EditorGUILayout.IntField(_level.BoardHeight, GUILayout.Width(30));
        EditorGUILayout.LabelField("Move count:", GUILayout.Width(75));
        _level.MoveCount = EditorGUILayout.IntField(_level.MoveCount, GUILayout.Width(30));
        GUILayout.EndHorizontal();
    }

    private void DrawGridView()
    {
        GUILayout.BeginVertical(GUI.skin.box);

        for (int h = _level.BoardHeight - 1; h >= 0; h--)
        {
            GUILayout.BeginHorizontal();

            for (int w = 0; w < _level.BoardWidth; w++)
            {
                int itemIndex = FindItemIndex(_level.Items, w, h);

                if (itemIndex != -1)
                {
                    if (GUILayout.Button(_textures[(int)_level.Items[itemIndex].Id], GUILayout.Width(30), GUILayout.Height(30)))
                    { }
                }
                else
                {
                    if (GUILayout.Button($"", GUILayout.Width(30), GUILayout.Height(30)))
                    { }
                }

            }
            EditorGUILayout.LabelField($"{h}", GUILayout.Width(30));
            GUILayout.EndHorizontal();
        }

        GUILayout.BeginHorizontal();

        for (int w = 0; w < _level.BoardWidth; w++)
        {
            EditorGUILayout.LabelField($"   {w}", GUILayout.Width(30));
        }

        GUILayout.EndHorizontal();
        GUILayout.EndVertical();
    }

    private void DrawHintSection()
    {
        EditorGUILayout.Space(5);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(100));

        if (GUILayout.Button("Add hint", GUILayout.Width(70), GUILayout.Height(20)))
        {
            HintInfo[] newHints = new HintInfo[_level.Hints.Length + 1];
            _level.Hints.CopyTo(newHints, 0);
            _level.Hints = newHints;
        }
        GUILayout.EndHorizontal();

        GUILayout.BeginVertical(GUI.skin.box);

        for (int i = 0; i < _level.Hints.Length; i++)
        {
            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("#" + (i + 1).ToString(), GUILayout.Width(25));
            EditorGUILayout.LabelField("  X:", GUILayout.Width(20));
            int x = EditorGUILayout.IntField(_level.Hints[i].X, GUILayout.Width(30));
            EditorGUILayout.LabelField("Y:", GUILayout.Width(14));
            int y = EditorGUILayout.IntField(_level.Hints[i].Y, GUILayout.Width(30));

            //GUILayout.EndHorizontal();

            //GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Direction:", GUILayout.Width(60));
            DirectionType direction = (DirectionType)EditorGUILayout.EnumPopup(_level.Hints[i].Direction, GUILayout.Width(70));
            EditorGUILayout.Space(5);

            HintInfo hintInfo = new HintInfo(x, y, direction);
            _level.Hints[i] = hintInfo;

            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                _level.Hints = DeleteElementFromHintnfoArray(_level.Hints, i);
                i--;
            }

            GUILayout.EndHorizontal();
        }

        GUILayout.EndVertical();
    }

    private void DrawItemInfoSection()
    {
        EditorGUILayout.Space(5);
        GUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("", GUILayout.Width(100));

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
                _level.Items = DeleteElementFromItemInfoArray(_level.Items, i);
                i--;
            }

            GUILayout.EndHorizontal();
        }
        GUILayout.EndVertical();
    }

    private int FindItemIndex(ItemInfo[] itemInfos, int x, int y)
    {
        for (int i = 0; i < itemInfos.Length; i++)
        {
            if (itemInfos[i].X == x && itemInfos[i].Y == y)
            {
                return i;
            }
        }

        return -1;
    }

    private ItemInfo[] AddElementToArray(ItemInfo[] array)
    {
        ItemInfo[] newArray = new ItemInfo[array.Length + 1];
        array.CopyTo(newArray, 0);

        if (array.Length > 0)
            newArray[array.Length] = newArray[array.Length - 1];

        return newArray;
    }

    private ItemInfo[] DeleteElementFromItemInfoArray(ItemInfo[] array, int index)
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

    private HintInfo[] DeleteElementFromHintnfoArray(HintInfo[] array, int index)
    {
        HintInfo[] newArray = new HintInfo[array.Length - 1];
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