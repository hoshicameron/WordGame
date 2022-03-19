using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

[CustomEditor(typeof(BoardData),false)]
[CanEditMultipleObjects]
[System.Serializable]
public class BoardDataDrawer : Editor
{
    private BoardData GameDataInstance=>target as BoardData;
    private ReorderableList _dataList;

    private void OnEnable()
    {
        InitializeReordableList(ref _dataList,"searchWords","Searching Words");
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        DrawColumnsRowsInputFields();
        EditorGUILayout.Space();

        ConvertToUpperButton();

        if(GameDataInstance.board!=null && GameDataInstance.columns >0 && GameDataInstance.rows>0)
            DrawBoardTable();

        EditorGUILayout.BeginHorizontal();
        ClearBoardButton();
        FillUpWithRandomLettersButton();
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        _dataList.DoLayoutList();

        serializedObject.ApplyModifiedProperties();
        if (GUI.changed)
        {
            EditorUtility.SetDirty(GameDataInstance);
        }
    }

    private void DrawColumnsRowsInputFields()
    {
        var columnsTemp = GameDataInstance.columns;
        var rowsTemp = GameDataInstance.rows;

        GameDataInstance.columns = EditorGUILayout.IntField("Columns", GameDataInstance.columns);
        GameDataInstance.rows = EditorGUILayout.IntField("Rows", GameDataInstance.rows);

        if ((GameDataInstance.columns != columnsTemp || GameDataInstance.rows != rowsTemp)
            && GameDataInstance.columns>0 && GameDataInstance.rows>0)
        {
            GameDataInstance.CreateNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        // style of Table
        var tableStyle=new GUIStyle("box");
        tableStyle.padding=new RectOffset(10,10,10,10);
        tableStyle.margin.left = 32;

        // style of header
        var headerColumnStyle=new GUIStyle();
        headerColumnStyle.fixedWidth = 35;

        //Style off columns
        var columnStyle=new GUIStyle();
        columnStyle.fixedWidth = 50;

        // Style of rows
        var rowStyle=new GUIStyle();
        rowStyle.fixedHeight = 25;
        rowStyle.fixedWidth = 40;
        rowStyle.alignment = TextAnchor.MiddleCenter;

        // text area style
        var textFieldStyle=new GUIStyle();
        textFieldStyle.normal.background=Texture2D.grayTexture;
        textFieldStyle.normal.textColor=Color.white;
        textFieldStyle.fontStyle = FontStyle.Bold;
        textFieldStyle.alignment = TextAnchor.MiddleCenter;

        EditorGUILayout.BeginHorizontal(tableStyle);

        for (int i = 0; i < GameDataInstance.columns; i++)
        {
            // ?
            EditorGUILayout.BeginVertical(i == -1 ? headerColumnStyle : columnStyle);

            for (int j = 0; j < GameDataInstance.rows; j++)
            {
                if (i >= 0 && j >= 0)
                {
                    EditorGUILayout.BeginHorizontal(rowStyle);
                    var character =
                        (string) EditorGUILayout.TextArea(GameDataInstance.board[i].row[j], textFieldStyle);

                    // if there is more than one character saved in row array, remove extra character
                    if (GameDataInstance.board[i].row[j].Length > 1)
                    {
                       character= GameDataInstance.board[i].row[j].Substring(0, 1);
                    }

                    GameDataInstance.board[i].row[j] = character;
                    EditorGUILayout.EndHorizontal();
                }
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndHorizontal();
    }

    private void InitializeReordableList(ref ReorderableList list, string propertyName, string listLable)
    {
        list = new ReorderableList(serializedObject, serializedObject.FindProperty(propertyName),
            true, true, true,true);

        list.drawHeaderCallback = rect => { EditorGUI.LabelField(rect, listLable); };

        var l = list;

        list.drawElementCallback = (rect, index, active, focused) =>
        {
            var element = l.serializedProperty.GetArrayElementAtIndex(index);
            rect.y += 2;

            EditorGUI.PropertyField(
                new Rect(rect.x, rect.y, EditorGUIUtility.labelWidth, EditorGUIUtility.singleLineHeight),
                element.FindPropertyRelative("word"), GUIContent.none);
        };

    }


    private void ConvertToUpperButton()
    {
        if (GUILayout.Button("To Upper"))
        {
            for (int i = 0; i < GameDataInstance.columns; i++)
            {
                for (int j = 0; j < GameDataInstance.rows; j++)
                {
                    var errorCounter = Regex.Matches(GameDataInstance.board[i].row[j], @"[a-z]").Count;

                    if (errorCounter > 0)
                        GameDataInstance.board[i].row[j] = GameDataInstance.board[i].row[j].ToUpper();
                }
            }

            foreach (var searchingWord in GameDataInstance.searchWords)
            {
                var errorCounter = Regex.Matches(searchingWord.word, @"[a-z]").Count;

                if (errorCounter > 0)
                    searchingWord.word = searchingWord.word.ToUpper();
            }
        }
    }

    private void ClearBoardButton()
    {
        if (GUILayout.Button("Clear Board"))
        {
            for (int i = 0; i < GameDataInstance.columns; i++)
            {
                for (int j = 0; j < GameDataInstance.rows; j++)
                {
                    GameDataInstance.board[i].row[j] = string.Empty;
                }
            }
        }
    }

    private void FillUpWithRandomLettersButton()
    {
        if (GUILayout.Button("Fill With Random Letters"))
        {
            for (int i = 0; i < GameDataInstance.columns; i++)
            {
                for (int j = 0; j < GameDataInstance.rows; j++)
                {
                    var errorCounter = Regex.Matches(GameDataInstance.board[i].row[j], @"[a-zA-Z ]").Count;
                    string letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                    int index = Random.Range(0, letters.Length);

                    if (errorCounter == 0)
                        GameDataInstance.board[i].row[j] = letters[index].ToString();
                }
            }
        }
    }
}
