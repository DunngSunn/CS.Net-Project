using UnityEditor;
using UnityEngine;

namespace VanThang
{
    [CustomEditor(typeof(ShapeData), false)]
    [CanEditMultipleObjects]
    [System.Serializable]
    public class ShapeDataDrawer : Editor
    {
        private ShapeData ShapeDataInstance => target as ShapeData;

    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        
        ClearBoardButton();
        EditorGUILayout.Space();
        
        DrawInputFields();
        EditorGUILayout.Space();

        if (ShapeDataInstance.columns > 0 && ShapeDataInstance.rows > 0)
        {
            DrawBoardTable();
        }

        serializedObject.ApplyModifiedProperties();

        if (GUI.changed)
        {
            EditorUtility.SetDirty(ShapeDataInstance);
        }
    }
    
    private void ClearBoardButton()
    {
        if (GUILayout.Button("Clear"))
        {
            ShapeDataInstance.ClearBoard();
        }
    }

    private void DrawInputFields()
    {
        var columnsTemp = ShapeDataInstance.columns;
        var rowsTemp = ShapeDataInstance.rows;

        ShapeDataInstance.shapeID = EditorGUILayout.IntField("Shape ID: ", ShapeDataInstance.shapeID);
        ShapeDataInstance.columns = EditorGUILayout.IntField("Columns: ", ShapeDataInstance.columns);
        ShapeDataInstance.rows = EditorGUILayout.IntField("Rows: ", ShapeDataInstance.rows);

        if ((ShapeDataInstance.columns != columnsTemp || ShapeDataInstance.rows != rowsTemp) && ShapeDataInstance.columns > 0 && ShapeDataInstance.rows > 0)
        {
            ShapeDataInstance.CreateNewBoard();
        }
    }

    private void DrawBoardTable()
    {
        EditorGUILayout.Space();

        var headerColumnStyle = new GUIStyle {fixedWidth = 65, alignment = TextAnchor.MiddleCenter};
        var rowStyle = new GUIStyle {fixedHeight = 22, alignment = TextAnchor.MiddleCenter};
        var dataFieldStyle = new GUIStyle(EditorStyles.miniButtonMid)
        {
            normal = {background = Texture2D.grayTexture}, 
            onNormal = {background = Texture2D.whiteTexture}
        };
        
        for (var row = 0; row < ShapeDataInstance.rows; row++) {
            EditorGUILayout.BeginHorizontal(headerColumnStyle);

            for (var column = 0; column < ShapeDataInstance.columns; column++) {
                EditorGUILayout.BeginHorizontal(rowStyle);
                var data = EditorGUILayout.Toggle(ShapeDataInstance.board[row].columns[column], dataFieldStyle);
                ShapeDataInstance.board[row].columns[column] = data;
                EditorGUILayout.EndHorizontal();
            }

            EditorGUILayout.EndHorizontal();
        }
    }
    }
}