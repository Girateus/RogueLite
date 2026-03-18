using UnityEditor;using UnityEngine;

[CustomEditor(typeof(DungeonGenerator))]
public class DungeoGeneratorEditor : Editor
{
    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        
        DungeonGenerator dungeonGenerator = (DungeonGenerator)target;

        DrawDefaultInspector();
        
        EditorGUILayout.Space(25);
        EditorGUILayout.Separator();

        if (GUILayout.Button("Generate Dungeon"))
        {
            dungeonGenerator.GenerateDungeon();
        }


    }

}
